using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private bool isGrounded;
    private int xInput;
    private bool jumpInput;
    private bool coyoteTime;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckGround();
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        StartCoyoteTime();
        xInput = player.InputHandler.NormalizedInputX;
        jumpInput = player.InputHandler.JumpInput;

        if (isGrounded && player.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else
        {
            player.CheckFlip(xInput);
            player.SetVelocityX(playerData.movementVelocity * xInput);

            player.PlayerAnimator.SetFloat("yVelocity", player.CurrentVelocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckCoyoteztime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void StartCoyoteTime()
    {
        coyoteTime = true;
    }
}
