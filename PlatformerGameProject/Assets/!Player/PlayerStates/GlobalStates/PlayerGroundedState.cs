using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//глобальное состояние игрока: На земле
public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    private bool jumpInput;
    //private bool dashInput;

    private bool isGrounded;
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
        player.JumpState.ResetAmountOfJumpsLeft();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //считываем ввод
        xInput = player.InputHandler.NormalizedInputX;
        jumpInput = player.InputHandler.JumpInput;
        //dashInput = player.InputHandler.DashInput;

        if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        //if (dashInput)
        //{
        //    stateMachine.ChangeState(player.DashState);
        //}
        else if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
