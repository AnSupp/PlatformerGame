using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private LayerMask m_WhatIsObstacle;  // A mask determining what is obstacle to the arrow
    [SerializeField] private float speed = 10f;
    private Rigidbody2D rb;
    private int arrowDamage;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    public void SetArrowDamage(int damage)
    {
        arrowDamage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       // EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
       // if (enemy != null)
       // {
       //     enemy.TakeDamage(arrowDamage);
       //     Destroy(gameObject);
       // }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CircleCollider2D collider2D = transform.GetChild(0).GetComponent<CircleCollider2D>();
        if (Physics2D.IsTouchingLayers(collider2D, m_WhatIsObstacle))
        {
            collider2D.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            StartCoroutine(DestroyOnTime());
        }
    }

    private IEnumerator DestroyOnTime()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

}
