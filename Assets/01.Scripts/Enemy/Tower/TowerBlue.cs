using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBlue : Tower
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
        UIManager.Instance.TowerAttack("마력의 흐름이 차단되었습니다.");
        StartCoroutine(BlueTowerAttack());
    }

    IEnumerator BlueTowerAttack()
    {
        GameManager.Instance.PlayerTrm.GetChild(0).GetComponent<PlayerOrbCounter>().RotationSpeedDown();
        UIManager.Instance.LockActive(true);
        yield return new WaitForSeconds(1.2f);
        GameManager.Instance.PlayerTrm.GetChild(0).GetComponent<PlayerOrbCounter>().rotationSpeed = 90;
        yield return new WaitForSeconds(6f);
        UIManager.Instance.LockActive(false);
        GameManager.Instance.PlayerTrm.GetChild(0).GetComponent<PlayerOrbCounter>().rotationSpeed = 180;
    }

    protected override IEnumerator EnemySpawn()
    {
        return base.EnemySpawn();
    }

    protected override void Die()
    {
        base.Die();
        UIManager.Instance.LockActive(false);
        GameManager.Instance.PlayerTrm.GetChild(0).GetComponent<PlayerOrbCounter>().rotationSpeed = 180;
        TowerManager.Instance.blueTower = true;
        Destroy(gameObject);
    }
}
