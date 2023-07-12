using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGreen: Tower
{
    [SerializeField]
    Collider2D col;
    Bounds bounds;
    LayerMask EnemyAndPlayer;
    float healValue;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(EnemySpawn());
        bounds = col.bounds;
        min = UIManager.Instance.min;
        max = UIManager.Instance.max;
        int i = LayerMask.GetMask("isEnemy");
        int j = LayerMask.GetMask("Player");
        EnemyAndPlayer = i | j;
    }


    public void TowerAttack()
    {
        UIManager.Instance.TowerAttack("생명력이 흘러 나옵니다.");
        StartCoroutine(DelayCoroutine(GreenTowerAttack));
    }

    public void GreenTowerAttack()
    {
        Collider2D[] colls = Physics2D.OverlapAreaAll(bounds.min, bounds.max, EnemyAndPlayer);
        healValue = Random.Range(6, 9);
        foreach (Collider2D col in colls)
        {
            if (col.CompareTag("Enemy"))
            {
                col.GetComponent<Enemy>().HpUp(col.GetComponent<Enemy>().hp / 100 * healValue);
            }
            else if(col.CompareTag("Player"))
            {
                UIManager.Instance.HpUp(UIManager.Instance.hpValue / 100 * healValue);
            }
            else
            {
                UIManager.Instance.bossHp += UIManager.Instance.bossHp / 100 * healValue;
            }
        }
    }

    protected override void Die()
    {
        base.Die();
        TowerManager.Instance.greenTower = true;
        Destroy(gameObject);
    }
}
