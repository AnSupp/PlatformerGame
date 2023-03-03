using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : EntityHealth
{
    private EnemyCombat enemyCombat;

    protected override void Awake()
    {
        base.Awake();
        enemyCombat = GetComponent<EnemyCombat>();
    }

    public override void TakeDamage(int damage)
    {
        enemyCombat.isAttacking = false;
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        GetComponent<EnemyAI>().enabled = false;
        base.Die();
    }
}
