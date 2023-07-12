using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRed : Tower
{
    [SerializeField]
    GameObject _meteorPrefab;

    [SerializeField]
    Collider2D col;
    Bounds bounds;

    public bool METEOR = false; // ����׿�

    protected override void Start()
    {
        base.Start();
        StartCoroutine(EnemySpawn());
        bounds = col.bounds;
        min = UIManager.Instance.min;
        max = UIManager.Instance.max;
       
    }

    protected override IEnumerator EnemySpawn()
    {
        return base.EnemySpawn();
    }

    public void TowerAttack()
    {
        UIManager.Instance.TowerAttack("������ ���� �����߽��ϴ�.");
        StartCoroutine(DelayCoroutine(RedTowerAttack));
    }

    public void RedTowerAttack()
    {
        int rand = Random.Range(40, 50);
        for (int i = 0; i < rand; i++)
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            Vector2 randomPosition = new Vector2(randomX, randomY);
            Instantiate(_meteorPrefab, randomPosition, Quaternion.identity);
        }
    }

    protected override IEnumerator DelayCoroutine(System.Action Callback)
    {
        return base.DelayCoroutine(Callback);
    }

    protected override void Die()
    {
        base.Die();
        TowerManager.Instance.redTower = true;
        Destroy(gameObject);
    }
}
