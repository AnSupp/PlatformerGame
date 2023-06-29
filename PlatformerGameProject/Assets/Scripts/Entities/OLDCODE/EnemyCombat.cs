using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : EntityCombat
{
    private void Hit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, hitLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<PlayerHealth>().TakeDamage(ATKDamage);
        }
    }
}