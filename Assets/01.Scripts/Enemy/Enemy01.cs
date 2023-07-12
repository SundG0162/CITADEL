using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01 : Enemy
{
    bool active = false;
    protected override void Die()
    {
        if (active) return;
        active = true;
        base.Die();
        Destroy(gameObject);
    }
}
