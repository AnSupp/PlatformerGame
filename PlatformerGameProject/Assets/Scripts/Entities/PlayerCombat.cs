using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : EntityCombat
{
    private PlayerMovementController movementController;

    [Header("Bow Attack")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private int bowATKDamage;
    [SerializeField] [Range(1f, 2f)] private float bowATKCooldown;
    [SerializeField] private Transform bowAttackPoint;
    private bool canBowAttack = true;

    protected override void Awake()
    {
        base.Awake();
        movementController = GetComponent<PlayerMovementController>();
    }

    private void Hit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, hitLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(ATKDamage);
        }
    }

    private void Update()
    {
        if (movementController.isDashing || (movementController.isJumping))
        {
            return;
        }

        if (isAttacking)
        {
            return;
        }

        if (Input.GetButtonDown("LightAttack") && canAttack)
        {
            StartCoroutine(Attack());
        }

        if (Input.GetButtonDown("BowAttack") && canBowAttack)
        {
            StartCoroutine(BowAttack());
        }

    }

    private IEnumerator BowAttack()
    {
        animator.SetTrigger("BowATK");
        isAttacking = true;

        canBowAttack = false;

        yield return new WaitForSeconds(bowATKCooldown);
        canBowAttack = true;

    }

    private void ArrowShot()
    {
        Instantiate(arrowPrefab, bowAttackPoint.position, bowAttackPoint.rotation).GetComponent<Arrow>().SetArrowDamage(bowATKDamage);
    }
}
