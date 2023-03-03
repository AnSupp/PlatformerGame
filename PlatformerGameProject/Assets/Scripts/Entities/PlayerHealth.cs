using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityHealth
{
    private PlayerCombat playerCombat;

    protected override void Awake()
    {
        base.Awake();
        playerCombat = GetComponent<PlayerCombat>();
    }

    public override void TakeDamage(int damage)
    {
        playerCombat.isAttacking = false;
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        GetComponent<PlayerMovementController>().enabled = false;
        playerCombat.enabled = false;
        base.Die();
    }
}
