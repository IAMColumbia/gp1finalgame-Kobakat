using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MovementState : MoveState
{
    Player player;
    public MovementState(Player Player) 
    {
        player = Player;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        base.CheckForChunksCurrentlyIn(player);

        CheckIfPlayerIsTryingToTurn();
        Accelerate();

        base.UpdatePosition(player);
        base.UpdateRectAndCheckForCollisions(player);
        base.CheckForCollisionWithOtherEntities(player);
    }

    public sealed override void OnStateEnter() { base.OnStateEnter(); }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    #region Logic Functions

    void Accelerate()
    {
        //Sprite is on the ground, they should have more control
        if(player.groundState is GroundedState)
            player.speed += player.xMoveDir * Time.deltaTime * player.acceleration;

        //Sprite is airborne, turning is harder
        else
            player.speed += (player.xMoveDir * Time.deltaTime * player.acceleration) / player.airborneMovementDamp;

        player.speed = Mathf.Clamp(player.speed, -player.maxSpeed, player.maxSpeed);
    }

    /// <summary>
    /// If they are, set the state to turning
    /// </summary>
    void CheckIfPlayerIsTryingToTurn()
    {
        //Check if they're grounded
        if(player.groundState is GroundedState)
        {
            //Check if their input direction and movement direction conflict
            if (player.xMoveDir > 0 && player.speed < 0
            || player.xMoveDir < 0 && player.speed > 0)
            {
                //Set the state
                player.SetState(ref player.moveState, new TurningState(player));
            }
        }
    }
    #endregion
}
