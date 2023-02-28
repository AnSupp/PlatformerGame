using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;

    private Animator entityAnimator;

    private void Awake()
    {
        entityAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        entityAnimator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        entityAnimator.SetBool("Dead", true);

        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EnemyAI>().enabled = false;

        this.enabled = false;
    }
}

