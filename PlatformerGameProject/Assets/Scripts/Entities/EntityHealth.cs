using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityHealth : MonoBehaviour
{
    protected Animator animator;

    public int maxHealth;
    protected int currentHealth;
    [HideInInspector] public bool isTakingDamage = false;
    private Rigidbody2D rb;
    //private RigidbodyConstraints2D originalConstraints;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //originalConstraints = rb.constraints;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        //rb.constraints = RigidbodyConstraints2D.FreezePosition;
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
        //rb.constraints = originalConstraints;
        isTakingDamage = false;
    }

    protected virtual void Die()
    {
        animator.SetBool("Dead", true);

        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        GetComponent<Collider2D>().enabled = false;     
        this.enabled = false;
    }
}


