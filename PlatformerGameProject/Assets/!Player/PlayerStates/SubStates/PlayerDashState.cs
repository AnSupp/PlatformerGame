using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    //private int xInput;
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        player.InputHandler.UseDashInput();
        player.SetVelocityDash(playerData.dashVelocity);
        isAbilityDone = true;
    }

    //public override void LogicUpdate()
    //{
    //    base.LogicUpdate();
    //
    //    xInput = player.InputHandler.NormalizedInputX;
    //
    //    if (!isExitingState)
    //    {
    //        if (xInput != 0)
    //        {
    //            stateMachine.ChangeState(player.RunState);
    //        }
    //        else if (isAnimationFinished)
    //        {
    //            stateMachine.ChangeState(player.IdleState);
    //        }
    //    }
    //}
}
