using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityCombat : MonoBehaviour
{
    protected Animator animator;

    [SerializeField] protected LayerMask hitLayer;

    [HideInInspector] public bool isAttacking = false;


    [Header("Attack")]
    [SerializeField] protected int ATKDamage;
    [SerializeField] [Range(0.85f, 3f)] private float ATKCooldown;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected float attackRange;
    [HideInInspector] public bool canAttack = true;


    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartAttack()
    {
        StartCoroutine(Attack());
    }

    protected IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        isAttacking = true;

        canAttack = false;

        yield return new WaitForSeconds(ATKCooldown);
        canAttack = true;

    }

    private void AttackOver() //запуск из анимации
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected() //для редактора
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
