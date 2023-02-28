using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private LayerMask playerLayer;

    [HideInInspector] public bool isAttacking = false;


    [Header("Attack")]
    [SerializeField] private int ATKDamage;
    [SerializeField] [Range(0.85f, 3f)] private float ATKCooldown;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [HideInInspector] public bool canAttack = true;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartAttack()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        isAttacking = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Entity>().TakeDamage(ATKDamage);
        }

        canAttack = false;

        yield return new WaitForSeconds(ATKCooldown);
        canAttack = true;

    }

    private void AttackOver()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected() //для редактора
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}