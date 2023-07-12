using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UseHitEffectMono : MonoBehaviour
{
    [SerializeField]
    protected LayerMask isEnemy;
    protected SpriteRenderer _spriteRenderer;
    [SerializeField]
    protected GameObject _lightningExplosion;
    [SerializeField]
    protected GameObject _explosionEffect;
    [SerializeField]
    protected GameObject _lightningEffect;
    [SerializeField]
    protected GameObject _weekLightningEffect;
    [SerializeField]
    protected GameObject _dieEffect;
    [SerializeField]
    protected GameObject _lightningOrbItem;
    [SerializeField]
    protected GameObject _fireOrbItem;
    [SerializeField]
    protected EnemySO _enemySO;
    [SerializeField]
    protected TowerSO _towerSO;
    [SerializeField] int rand = 100;
    
    [SerializeField]
    public float hp;
    [SerializeField]
    protected int score;
    Collider2D _col;

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _col = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        if (_enemySO != null)
        {
            score = _enemySO.score;
            hp = _enemySO.hp;
        }
        else
        {
            score = _towerSO.score;
            hp = _towerSO.hp;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FireOrb"))
        {
            if (collision.transform.parent == false)
            {
                FireHit(range: 4f, effect: _explosionEffect, damage: 80f, collision.transform.position);
                TimeController.Instance.SetTimeFreeze(freezeValue: 0.3f, beforeDelay: 0, freezeTime: 0.1f);
                Destroy(collision.gameObject);
            }
            else
                FireHit(range: 2f, effect: _explosionEffect, damage:15f, collision.transform.position);
            if(hp <= 0)
            {
                UIManager.Instance.ScoreUp(score);
                Die();
            }
        }
        else if (collision.CompareTag("LightningOrb"))
        {
            if (collision.transform.parent == false)
            {
                LightningHit(range: 4f, effect: _lightningExplosion, damage: 100f);
                TimeController.Instance.SetTimeFreeze(freezeValue: 0.5f, beforeDelay: 0.05f, freezeTime: 0.1f);
                Destroy(collision.gameObject);
            }
            else
                LightningHit(range: 2f, effect: _lightningEffect, damage: 20f);
            
        }
    }

    protected virtual void FireHit(float range, GameObject effect, float damage, Vector3 hitPos)
    {
        hp -= damage;
        GameObject obj = Instantiate(effect, hitPos, Quaternion.identity);
        Destroy(obj, 1f);
        FireDamage(damage);
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, range, isEnemy);
        foreach (Collider2D col in colls)
        {
            if (col.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.FireDamage(10f);
            }
        }
    }


    protected virtual void LightningHit(float range, GameObject effect, float damage)
    {
        GameObject obj = Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(obj, 1f);
        LightningDamage(20, damage);
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, range, isEnemy);
        foreach (Collider2D col in colls)
        {
            if (col.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.LightningDamage(15f,0);
            }
        }
    }

    public virtual void LightningDamage(float delayTime, float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            UIManager.Instance.ScoreUp(score);
            Die();
        }
        GameObject obj = Instantiate(_weekLightningEffect, transform.position, Quaternion.identity);
        Destroy(obj, 1f);
        StopCoroutine(LightningHitEffect(delayTime));
        StartCoroutine(LightningHitEffect(delayTime));
    }
    protected virtual void FireDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            UIManager.Instance.ScoreUp(score);
            Die();
        }
        GameObject obj = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        StopCoroutine("FireHitEffect");
        StartCoroutine("FireHitEffect");
    }

    protected virtual IEnumerator FireHitEffect()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        _spriteRenderer.color = Color.white;
        yield return null;
    }
    protected virtual IEnumerator LightningHitEffect(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        _spriteRenderer.color = new Color(127, 248, 255);
        yield return new WaitForSeconds(0.3f);
        _spriteRenderer.color = Color.white;
        yield return null;
    }

    protected virtual void Die()
    {
        
        rand = Random.Range(0, 100);
        if(rand <= 3)
        {
            int rand2 = Random.Range(0, 10);
            if(rand2 <= 6)
            {
                Instantiate(_fireOrbItem, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_lightningOrbItem, transform.position, Quaternion.identity);
            }
        }
    }
}
