using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class Boss : UseHitEffectMono
{
    [SerializeField]
    GameObject _enemyBullet;
    [SerializeField]
    GameObject _meteorPrefab;
    [SerializeField]
    GameObject _cameraShake;
    [SerializeField]
    LayerMask isPlayer;

    [SerializeField]
    ScoreSO _scoreSO;

    Transform _playerTransform;

    Animator _animator;

    public float moveSpeed = 5f;

    bool isAttack;

    bool isSpell;

    [SerializeField]
    List<int> tower = new List<int>();

    protected override void Awake()
    {
        base.Awake();

        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();

        _animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SpellCoroutine());
        if(TowerManager.Instance.redTower)
        {
            tower.Add(1);
        }
        if (TowerManager.Instance.blueTower)
        {
            tower.Add(2);

        }
        if (TowerManager.Instance.blackTower)
        {
            tower.Add(3);

        }
        if (TowerManager.Instance.greenTower)
        {
            tower.Add(4);

        }
    }

    private void Update()
    {
        UIManager.Instance.bossHp = hp;
        UIManager.Instance.BossHpInit();
        if (UIManager.Instance.bossHp <= 0)
        {
            _animator.SetBool("Attack", false );
            _animator.SetBool("Spell", false);
            _animator.SetBool("Dead", true);
            StartCoroutine(DeadCoroutine());
            isAttack = true;
        }
        if (isAttack || isSpell) return;

        int rand = Random.Range(0, 30);
        transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, moveSpeed * Time.deltaTime);
        if (transform.position.x - _playerTransform.position.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (transform.position.x - _playerTransform.position.x >= 0)
        {
            _spriteRenderer.flipX = false;
        }

        if (Vector2.Distance(transform.position, _playerTransform.position) <= 3f)
        {
            isAttack = true;
            _animator.SetBool("Attack", true);
            StartCoroutine(AttackEnd());
        }

       
    }

    IEnumerator AttackEnd()
    {
        yield return new WaitForSeconds(1f);
        isAttack = false;
        _animator.SetBool("Attack", false);
    }

    public void Attack()
    {
        if (_spriteRenderer.flipX == true)
        {
            Collider2D col = Physics2D.OverlapBox(new Vector2(transform.position.x + 3f, transform.position.y), new Vector2(2.5f,2.5f), isPlayer);
            if(col != null && GameManager.Instance.isAdding)
            {
                UIManager.Instance.BeAttack(20);
            }
        }
        else if(_spriteRenderer.flipX == false)
        {
            Collider2D col = Physics2D.OverlapBox(new Vector2(transform.position.x - 3f, transform.position.y), new Vector2(2.5f, 2.5f), isPlayer);
            if (col != null && GameManager.Instance.isAdding)
            {
                UIManager.Instance.BeAttack(20);
            }
        }
    }

    IEnumerator SpellEnd()
    {
        yield return new WaitForSeconds(1f);    
        isSpell = false;
        _animator.SetBool("Spell", false);
    }


    bool active = false;
    IEnumerator DeadCoroutine()
    {
        if (active) yield break;
        active = true;
        _scoreSO.score = UIManager.Instance.score;
        if(_scoreSO.bestScore < _scoreSO.score)
        {
            _scoreSO.bestScore = _scoreSO.score;
        }
        Destroy(TowerManager.Instance.transform.GetChild(0).gameObject);
        TowerManager.Instance.transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameObject.Find("GlobalLight").GetComponent<Light2D>().intensity = 0.7f;
        TorchLightManager.Instance.LightDisble();
        Instantiate(_cameraShake);
        Destroy(gameObject);
        BGMManager.Instance.BGMOff();
    }


    public void RedAttack()
    {
        for(int i = 0; i < 10; i++)
        {
            Instantiate(_meteorPrefab, GetRandomPosition(), Quaternion.identity);
        }
    }

    public Vector3 GetRandomPosition()
    {
        float radius = 3f;
        Vector3 playerPosition = _playerTransform.position;

        float a = playerPosition.x;
        float b = playerPosition.y;

        float x = Random.Range(-radius + a, radius + a);
        float y_b = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(x - a, 2));
        y_b *= Random.Range(0, 2) == 0 ? -1 : 1;
        float y = y_b + b;

        Vector3 randomPosition = new Vector3(x, y, 0);

        return randomPosition;
    }


    IEnumerator BlueAttack()
    {
        GameManager.Instance.PlayerTrm.GetChild(0).GetComponent<PlayerOrbCounter>().RotationSpeedDown();
        UIManager.Instance.LockActive(true);
        GameManager.Instance.PlayerTrm.GetChild(0).GetComponent<PlayerOrbCounter>().rotationSpeed -= 90;
        yield return new WaitForSeconds(6f);
        UIManager.Instance.LockActive(false);
        GameManager.Instance.PlayerTrm.GetChild(0).GetComponent<PlayerOrbCounter>().rotationSpeed += 90;
    }

    public void BlackAttak()
    {
        TorchLightManager.Instance.BlackTowerAttack();
    }

    public void GreenAttack()
    {
        HpUp(200);
    }


    IEnumerator SpellCoroutine()
    {
        isAttack = true;
        yield return new WaitForSeconds(2f);
        isAttack = false;
        yield return new WaitForSeconds(8f);
        while(true)
        {
            isSpell = true;
            _animator.SetBool("Spell", true);
            int rand = Random.Range(0, 3);
            if (tower[rand] == 1)
            {
                RedAttack();
            }
            if (tower[rand] == 2)
            {
                BlueAttack();
            }
            if (tower[rand] == 3)
            {
                StartCoroutine(BlueAttack());
            }
            if (tower[rand] == 4)
            {
                GreenAttack();
            }
            StartCoroutine(SpellEnd());
            yield return new WaitForSeconds(9f);
        }
        
    }

    public void HpUp(float hp)
    {
        this.hp += hp;
    }
}
