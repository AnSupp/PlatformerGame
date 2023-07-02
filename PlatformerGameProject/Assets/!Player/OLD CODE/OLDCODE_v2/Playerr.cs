using UnityEngine;
 
public class Playerr : MonoBehaviour
{
    [Header("Movement Vars")]
    [SerializeField] private float runSpeed = 0.15f;
    [SerializeField] private float jumpForce = 3;
    //[SerializeField] private float dashForce = 2.6f;
    //[SerializeField] private float dashTime = 0.3f;

    private float _walkTime = 0;
    private float _walkCooldown = 0.1f;

    private MoveState playerMoveState = MoveState.Idle;
    private DirectionState playerDirectionState = DirectionState.Right;
    private Transform playerTransform;
    private Rigidbody2D playerRigidbody;
    private Animator playerAnimatorController;


    private void Idle()
    {
        playerMoveState = MoveState.Idle;
        playerAnimatorController.Play("Idle");
    }

    public void Move(float horizontalMove)
    {
        playerRigidbody.velocity = ((playerDirectionState == DirectionState.Right ? Vector2.right : Vector2.left) * 5 * runSpeed);

        if ((horizontalMove > 0) && (playerDirectionState == DirectionState.Left))
        {
            playerDirectionState = DirectionState.Right;
            playerTransform.Rotate(0f, 180f, 0f);
        }
        else if ((horizontalMove < 0) && (playerDirectionState == DirectionState.Right))
        {
            playerDirectionState = DirectionState.Left;
            playerTransform.Rotate(0f, 180f, 0f);
        }

        if (playerMoveState != MoveState.Jump)
        {
            playerMoveState = MoveState.Walk;
            _walkTime = _walkCooldown;
            playerAnimatorController.Play("Run");
        }
    }

    public void Jump()
    {
        if (playerMoveState != MoveState.Jump)
        {
            playerRigidbody.velocity = (Vector3.up * jumpForce);
            playerMoveState = MoveState.Jump;
            playerAnimatorController.Play("Jump");
        }
    }

    public void Dash()
    {

    }

    private void Start()
    {
        playerTransform = GetComponent<Transform>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimatorController = GetComponent<Animator>();
        playerDirectionState = transform.localScale.x > 0 ? DirectionState.Right : DirectionState.Left;
    }

    private void FixedUpdate()
    {
        Debug.Log(playerMoveState);
        if (playerMoveState == MoveState.Jump)
        {
            if (playerRigidbody.velocity == Vector2.zero)
            {
                Idle();
            }
        }
        else if (playerMoveState == MoveState.Walk)
        {
            //
            _walkTime -= Time.deltaTime;
            if (_walkTime <= 0)
            {
                Idle();
            }
        }
    }

    enum DirectionState
    {
        Right,
        Left
    }

    enum MoveState
    {
        Idle,
        Walk,
        Jump,
        Dash
    }
}
