using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator playerAnimator;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private GameObject bulletPrefab;
  
    private bool isAttacking = false;
    
    
    [Header("Light Attack")]
    [SerializeField] private Transform lightAttackPoint;
    [SerializeField] [Range(0.85f, 1.25f)] private float lightATKCooldown = 0.85f;
    [SerializeField] private float lightAttackRange;
    [SerializeField] private int lightATKDamage;
    private bool canLightAttack = true;

    [Header("Bow Attack")]
    [SerializeField] private Transform bowAttackPoint;
    [SerializeField] [Range(1f, 2f)] private float bowATKCooldown;
    private bool canBowAttack = true;

    

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAttacking)
        {
            if (Input.GetButtonDown("LightAttack") && canLightAttack)
            {
                StartCoroutine(LightAttack());
            }

            if (Input.GetButtonDown("BowAttack") && canBowAttack)
            {
                StartCoroutine(BowAttack());
            }
        }
    }

    private IEnumerator LightAttack()
    {
        playerAnimator.SetTrigger("LightATK1");

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

        canBowAttack = false;

        yield return new WaitForSeconds(bowATKCooldown);
        canBowAttack = true;

    }

    private void ArrowShot()
    {
        Instantiate(bulletPrefab, bowAttackPoint.position, bowAttackPoint.rotation);
    }

    private void OnDrawGizmosSelected() //для редактора
    {
        Gizmos.DrawWireSphere(lightAttackPoint.position, lightAttackRange);
    }
}
