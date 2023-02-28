using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator playerAnimator;
    private PlayerMovementController movementController;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private GameObject arrowPrefab;
  
    [HideInInspector] public bool isAttacking = false;
    
    
    [Header("Light Attack")]
    [SerializeField] private int lightATKDamage;
    [SerializeField] [Range(0.85f, 1.25f)] private float lightATKCooldown = 0.85f;
    [SerializeField] private Transform lightAttackPoint; 
    [SerializeField] private float lightAttackRange;
    private bool canLightAttack = true;

    [Header("Bow Attack")]
    [SerializeField] private int bowATKDamage;
    [SerializeField] [Range(1f, 2f)] private float bowATKCooldown;
    [SerializeField] private Transform bowAttackPoint;
    private bool canBowAttack = true;

    

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        movementController = GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        if (movementController.isDashing || (movementController.isJumping))
        {
            return;
        }

        if (isAttacking)
        {
            return;
        }

        if (Input.GetButtonDown("LightAttack") && canLightAttack)
        {
            StartCoroutine(LightAttack());
        }

        if (Input.GetButtonDown("BowAttack") && canBowAttack)
        {
            StartCoroutine(BowAttack());
        }
        
    }

    private IEnumerator LightAttack()
    {
        playerAnimator.SetTrigger("LightATK1");
        isAttacking = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(lightAttackPoint.position, lightAttackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Entity>().TakeDamage(lightATKDamage);
        }

        canLightAttack = false;

        yield return new WaitForSeconds(lightATKCooldown);
        canLightAttack = true;

    }

    private IEnumerator BowAttack()
    {
        playerAnimator.SetTrigger("BowATK");
        isAttacking = true;

        canBowAttack = false;

        yield return new WaitForSeconds(bowATKCooldown);
        canBowAttack = true;

    }

    private void ArrowShot()
    {
        Instantiate(arrowPrefab, bowAttackPoint.position, bowAttackPoint.rotation).GetComponent<Arrow>().SetArrowDamage(bowATKDamage);
    }

    private void AttackOver()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected() //для редактора
    {
        Gizmos.DrawWireSphere(lightAttackPoint.position, lightAttackRange);
    }
}
