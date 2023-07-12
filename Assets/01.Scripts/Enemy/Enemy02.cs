using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02 : Enemy
{
    [SerializeField]
    private GameObject _enemyBullet;
    bool active = false;
    protected override void Die()
    {
        if (active) return;
        active = true;
        base.Die();
        for (int i = 0; i < 8; i++)
        {
            GameObject obj = Instantiate(_enemyBullet, transform.position, Quaternion.identity);
            obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(300f * Mathf.Cos(Mathf.PI * 2 * i / 8), 300f * Mathf.Sin(Mathf.PI * 2 * i / 8)));

            obj.transform.Rotate(new Vector3(0, 0, 360f * i / 8 - 90));
        }
        Destroy(gameObject);
    }
}
