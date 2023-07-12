using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Enemy : UseHitEffectMono
{

    private float moveSpeed = 4f;

    Transform _playerTransform;




    protected override void Awake()
    {
        base.Awake();
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    protected override void Start()
    {
        base.Start();
        hp += UIManager.Instance.enemyHpValue;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, moveSpeed * Time.deltaTime);
        if(transform.position.x - _playerTransform.position.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if(transform.position.x - _playerTransform.position.x >= 0)
        {
            _spriteRenderer.flipX = false;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UIManager.Instance.BeAttack(_enemySO.damage);
            Destroy(gameObject);
        }
    }

    protected override void FireDamage(float damage)
    {
        base.FireDamage(damage);
    }

    protected override void FireHit(float range, GameObject effect, float damage,Vector3 hitPos)
    {
        base.FireHit(range, effect, damage, hitPos);
    }

    protected override IEnumerator FireHitEffect()
    {
        return base.FireHitEffect();
    }

    public override void LightningDamage(float delayTime, float damage)
    {
        base.LightningDamage(delayTime, damage);
    }

    protected override void LightningHit(float range, GameObject effect, float damage)
    {
        base.LightningHit(range, effect, damage);
    }

    protected override IEnumerator LightningHitEffect(float delayTime)
    {
        return base.LightningHitEffect(delayTime);
    }

    public void HpUp(float hp)
    {
        this.hp += hp;
    }

    
}
