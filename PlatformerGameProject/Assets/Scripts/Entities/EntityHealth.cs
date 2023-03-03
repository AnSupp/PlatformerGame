using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityHealth : MonoBehaviour
{
    protected Animator animator;

    public int maxHealth;
    protected int currentHealth;
    [HideInInspector] public bool isTakingDamage = false;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        isTakingDamage = true;
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void TakingDamageOver()
    {
        isTakingDamage = false;
    }

    protected virtual void Die()
    {
        animator.SetBool("Dead", true);

        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        GetComponent<Collider2D>().enabled = false;

        this.enabled = false;
    }
}


