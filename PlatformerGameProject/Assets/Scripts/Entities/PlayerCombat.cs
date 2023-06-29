using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : EntityCombat
{
    private PlayerMovementController movementController;
    private PlayerHealth playerHealth;

    [Header("Bow Attack")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private int bowATKDamage;
    [SerializeField] [Range(1f, 2f)] private float bowATKCooldown;
    [SerializeField] private Transform bowAttackPoint;
    private bool canBowAttack = true;


    private int lightAttackCombo = 0;

    protected override void Awake()
    {
        base.Awake();
        movementController = GetComponent<PlayerMovementController>();
        playerHealth = GetComponent<PlayerHealth>();
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
        if (playerHealth.isTakingDamage)
        {
            return;
        }

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
        else if (Input.GetButtonDown("BowAttack") && canBowAttack)
        {
            StartCoroutine(BowAttack());
        }

    }

    protected  IEnumerator LightAttack()
    {
        isAttacking = true;
        canAttack = false;

        switch (lightAttackCombo)
        {
            case 0:
                animator.SetTrigger("LightAttack1");
                lightAttackCombo++;
                break;
            case 1:
                animator.SetTrigger("LightAttack2");
                lightAttackCombo++;
                break;
            case 2:
                animator.SetTrigger("LightAttack3");
                lightAttackCombo = 0;
                break;
            default:
                lightAttackCombo = 0;
                break;
        }

        yield return new WaitForSeconds(0.2f);
        canAttack = true;
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
