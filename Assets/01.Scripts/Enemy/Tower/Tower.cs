using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Tower : UseHitEffectMono
{
    public float delayTime;

    public float min;
    public float max;
    [SerializeField]
    GameObject _enemyPrefab01;
    [SerializeField]
    GameObject _enemyPrefab02;
    [SerializeField]
    GameObject _towerExplosion;

    protected virtual IEnumerator EnemySpawn()
    {
        while (!GameManager.Instance.isGameStoping)
        {
            delayTime = Random.Range(UIManager.Instance.min, UIManager.Instance.max);
            if (UIManager.Instance.score >= 1000)
            {
                int rand = Random.Range(0, 10);
                if(rand <= 3)
                {
                    Instantiate(_enemyPrefab02, new Vector3(transform.position.x, transform.position.y - 2), Quaternion.identity);
                }
                else
                {
                    Instantiate(_enemyPrefab01, new Vector3(transform.position.x, transform.position.y - 2), Quaternion.identity);
                }
            }
            else
            {
                Instantiate(_enemyPrefab01, new Vector3(transform.position.x, transform.position.y - 2), Quaternion.identity);
            }
            yield return new WaitForSeconds(delayTime);
        }
    }

    protected virtual IEnumerator DelayCoroutine(Action Callback)
    {
        yield return new WaitForSeconds(1.2f);
        Callback?.Invoke();
    }

    bool active = false;
    protected override void Die()
    {
        if (active) return;
        active = true;
        base.Die();
        TimeController.Instance.SetTimeFreeze(freezeValue: 0.2f, beforeDelay: 0.1f, freezeTime: 0.5f);
        if (transform.parent.GetComponent<TowerManager>().towerCount == 2)
        {
            BossManager.Instance.BossSpawn();
        }
        GameObject obj = Instantiate(_towerExplosion, transform.position, Quaternion.identity);
        Destroy(obj, 2f);
    }
}
