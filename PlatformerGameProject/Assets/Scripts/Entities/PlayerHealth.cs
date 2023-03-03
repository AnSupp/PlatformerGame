using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityHealth
{
    private PlayerCombat playerCombat;
    [SerializeField] private GameObject bloodPoint;

    protected override void Awake()
    {
        base.Awake();
        playerCombat = GetComponent<PlayerCombat>();
    }

    public override void TakeDamage(int damage)
    {
        playerCombat.isAttacking = false;
        bloodPoint.GetComponent<Animator>().Play("BloodAnimation");
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        GetComponent<PlayerMovementController>().enabled = false;
        playerCombat.enabled = false;
        base.Die();
    }
}
