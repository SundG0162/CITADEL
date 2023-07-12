using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBlack : Tower
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine(EnemySpawn());
        min = UIManager.Instance.min;
        max = UIManager.Instance.max;
    }

    public void TowerAttack()
    {
        UIManager.Instance.TowerAttack("æÓµ“¿Ã ≥ª∑¡æ…æ“Ω¿¥œ¥Ÿ.");
        StartCoroutine(DelayCoroutine(TorchLightManager.Instance.BlackTowerAttack));
    }

    protected override IEnumerator DelayCoroutine(System.Action Callback)
    {
        return base.DelayCoroutine(Callback);
    }

    protected override void Die()
    {
        base.Die();
        TowerManager.Instance.blackTower = true;
        Destroy(gameObject);
    }
}
