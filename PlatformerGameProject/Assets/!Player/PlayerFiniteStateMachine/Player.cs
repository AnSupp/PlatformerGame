using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Vars
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set;  }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }

    [SerializeField] private PlayerData playerData;
    #endregion

    #region Player Components
    public Animator PlayerAnimator { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D PlayerRigidbody { get; private set; }
    #endregion

    
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Transform wallChecker;

    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    private Vector2 tempVector2;

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        RunState = new PlayerRunState(this, StateMachine, playerData, "run");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
    }

    private void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        InputHandler = GetComponent<PlayerInputHandler>();
        StateMachine.Initialize(IdleState);
        FacingDirection = 1;
    }

    private void Update()
    {
        CurrentVelocity = PlayerRigidbody.velocity; //возвращает скорость на начало каждого кадра
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void SetVelocityX (float velocity)
    {
        tempVector2.Set(velocity, CurrentVelocity.y);
        PlayerRigidbody.velocity = tempVector2;
        CurrentVelocity = tempVector2;
    }

    public void SetVelocityY(float velocity)
    {
        tempVector2.Set(CurrentVelocity.x, velocity);
        PlayerRigidbody.velocity = tempVector2;
        CurrentVelocity = tempVector2;
    }

    #region Check Functions
    public void CheckFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundChecker.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckWall()
    {
        return Physics2D.Raycast(wallChecker.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    #endregion

    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }

    private void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    private void AnimationFinishedTrigger()
    {
        StateMachine.CurrentState.AnimationFinishedTrigger();
    }
}
