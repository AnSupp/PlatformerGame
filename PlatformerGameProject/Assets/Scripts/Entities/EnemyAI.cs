using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    private EnemyCombat enemyCombat;
    private EnemyHealth enemyHealth;

    [Header("Pathfinding")]
    [SerializeField] private Transform target;
    [SerializeField] private float activateDistance = 50f;
    [SerializeField] private float pathUpdateSeconds = 0.5f;
    [SerializeField] private float attackDistance;

    [Header("Physics")]
    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private float jumpNodeHeightRequirement = 0.8f;
    [SerializeField] private float jumpModifier = 0.3f;
    [SerializeField] private float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    [SerializeField] private bool followEnabled = true;
    [SerializeField] private bool jumpEnabled = true;
    [SerializeField] private bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    private Vector2 currentVelocity;
    private RaycastHit2D isGrounded;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Animator animator;

    private void OnDrawGizmosSelected() //для редактора
    {
        Gizmos.DrawWireSphere(transform.position, activateDistance);
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    private void Awake()
    {
        enemyCombat = GetComponent<EnemyCombat>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if (enemyHealth.isTakingDamage)
        {
            return;
        }

        if (enemyCombat.isAttacking)
        {
            return;
        }

        if (TargetInAttackDistance() && enemyCombat.canAttack) //если игрок в зоне поражения и можем атаковать - атакуем
        {
            enemyCombat.StartAttack();
        }

        if (TargetInAttackDistance()) //в любом случае, если игрок в зоне поражения не двигаемся
        {
            Flip();
            rb.velocity = Vector2.zero;
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            return;
        }

        if (TargetInFollowDistance() && followEnabled)
        {
            PathFollow();
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInFollowDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        // Jump
        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                rb.AddForce(Vector2.up * speed * jumpModifier);
            }
        }

        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref currentVelocity, 0.5f);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void Flip()
    {
        Vector3 facingDirection = (target.transform.position - transform.position).normalized;
        if (facingDirection.x > 0)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (facingDirection.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private bool TargetInFollowDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private bool TargetInAttackDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < attackDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
