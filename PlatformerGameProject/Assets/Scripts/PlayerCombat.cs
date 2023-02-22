using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator playerAnimator;
    public Transform lightAttackPoint;
    public float lightAttackRange;

    public LayerMask enemyLayer;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("LightAttack"))
        {
            Attack();         
        }
        
    }

    private void Attack()
    {
        playerAnimator.SetTrigger("LightATK1");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(lightAttackPoint.position, lightAttackRange, enemyLayer);
    
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("Damaged");
        }
    
    }

    private void OnDrawGizmosSelected() //��� ���������
    {
        Gizmos.DrawWireSphere(lightAttackPoint.position, lightAttackRange);
    }
}
