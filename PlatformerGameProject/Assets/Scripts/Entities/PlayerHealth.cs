using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PlayerHealth : EntityHealth
{
    private PlayerCombat playerCombat;
    [SerializeField] private PlayerHealthBar healthBar;
    [SerializeField] private UnityEvent HitEvent;

    protected override void Awake()
    {
        base.Awake();
        playerCombat = GetComponent<PlayerCombat>();
    }

    public override void TakeDamage(int damage)
    {
        playerCombat.isAttacking = false;
        HitEvent.Invoke();

        base.TakeDamage(damage);
        healthBar.SetHealth(currentHealth);
    }

    protected override void Die()
    {
        GetComponent<PlayerMovementController>().enabled = false;
        playerCombat.enabled = false;
        base.Die();
    }
}
