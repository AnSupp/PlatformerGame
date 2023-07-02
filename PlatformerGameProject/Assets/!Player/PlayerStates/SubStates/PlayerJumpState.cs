using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int amountOfJumpsLeft;
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void EnterState()
    {
        base.EnterState();

        player.SetVelocityY(playerData.jumpVelocity);
        isAbilityDone = true;
        amountOfJumpsLeft--;
    }

    public bool CanJump()
    {
        return amountOfJumpsLeft > 0;
    }

    public void ResetAmountOfJumpsLeft()
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public void DecreaseAmountOfJumpsLeft()
    {
        amountOfJumpsLeft--;
    }
}
