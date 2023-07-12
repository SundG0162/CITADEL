using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerOrb : MonoBehaviour
{
    [SerializeField]
    public float range;
    [SerializeField]
    LayerMask isEnemy;
    private float speed;
    private Vector2 dir;
    void Update()
    {
        transform.position +=(Vector3)(dir * speed * Time.deltaTime);
    }
    public void Launch(Vector2 enemyPos)
    {
        this.dir = ((Vector3)enemyPos - transform.position).normalized;
        StartCoroutine(LaunchCoroutine());
    }

    IEnumerator LaunchCoroutine()
    {
        float percent = 0;
        float current = 0;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.5f;
            speed = Mathf.Lerp(5f,30f,easeInQuart(percent));
            yield return null;
        }
    }
    float easeInQuart(float x)
    {
        return x * x * x * x;
    }
}
