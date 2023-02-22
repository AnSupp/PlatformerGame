using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
	private Rigidbody2D playerRigidbody;
	private Animator playerAnimator;
	[SerializeField] private Collider2D playerHitCollider;

	//Проверка земли под ногами
	[SerializeField] private LayerMask m_WhatIsGround;  // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;   // A position marking where to check if the player is grounded.
	private  const float k_GroundedRadius = 0.05f;       // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;                            // Whether or not the player is grounded.

	//Движение
	private bool isFacingRight = true;          // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;
	private float movementSmoothing = 0.05f;    // How much to smooth out the movement
	private bool airControl = true;             // Whether or not a player can steer while jumping;

	private float horizontalMove = 0f;
	private bool jump = false;
	private bool isJumping = false;
	private bool isDashing = false;
	private bool canDash = true;

	[Header("Movement Vars")]
	[Range(0.1f, 1f)] [SerializeField] private float runSpeed = 1f;
	[Range(1, 4f)] [SerializeField] private float jumpForce = 2.5f;

	[Range(1, 4f)] [SerializeField] private float dashForce = 2.5f;
	[Range(1, 4f)] [SerializeField] private float dashTime = 0.2f;
	[Range(1, 10f)] [SerializeField] private float dashCooldown = 2f;

	private void Awake()
	{
		playerRigidbody = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (isDashing)
		{
			return;
		}
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		playerAnimator.SetFloat("Speed", Mathf.Abs(horizontalMove));


		if (Input.GetButtonDown("Jump"))
		{
			isJumping = true;
			jump = true;
			playerAnimator.SetBool("Jump", true);
		}

		if (isJumping)
		{
			return;
		}


		if ((Input.GetButtonDown("Dash")) && canDash)
		{
			StartCoroutine(Dash());
			playerAnimator.SetBool("Dash", true);
		}
	}

	private void FixedUpdate()
	{
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
			}
		}

		Move();
		jump = false;
	}


	public void Move()
	{
		if (isDashing)
		{
			return;
		}
		//only control the player if grounded or airControl is turned on
		if ((m_Grounded || airControl))
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(horizontalMove * 10f, playerRigidbody.velocity.y);
			// And then smoothing it out and applying it to the character
			playerRigidbody.velocity = Vector3.SmoothDamp(playerRigidbody.velocity, targetVelocity, ref velocity, movementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (horizontalMove > 0 && !isFacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (horizontalMove < 0 && isFacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			playerRigidbody.velocity = (Vector3.up * jumpForce);
		}
	}


	private IEnumerator Dash()
	{
		if (m_Grounded)
		{
			canDash = false;
			isDashing = true;
			playerRigidbody.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
			playerHitCollider.enabled = false;

			yield return new WaitForSeconds(dashTime);
			playerAnimator.SetBool("Dash", false);
			isDashing = false;
			playerHitCollider.enabled = true;

			yield return new WaitForSeconds(dashCooldown);
			Debug.Log("DashReady");							//////////////////////////////////////////////////////////////
			canDash = true;

		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		isFacingRight = !isFacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void OnCollisionEnter2D(Collision2D collision) //костыль, чтобы избежать повторной анимации прыжка в воздухе при повторном нажатии Space
	{
		if ((collision.gameObject.layer == 6) || (collision.gameObject.layer == 8)  || (playerRigidbody.velocity.y == 0))    //слой Ground
		{
			jump = false;
			isJumping = false;
			playerAnimator.SetBool("Jump", false);
		}
	}

	private void OnDrawGizmosSelected() //для редактора
	{
		Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundedRadius);
	}
}
