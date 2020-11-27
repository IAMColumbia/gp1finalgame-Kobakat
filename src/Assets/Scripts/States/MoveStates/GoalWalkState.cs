using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalWalkState : State, IMoveState
{
    bool hasHitBlock;
    public GoalWalkState(Player sprite)
    {
        this.sprite = sprite;
        this.hasHitBlock = false;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        UpdatePosition();
        UpdateRectAndCheckForCollisions();

        if(!hasHitBlock)
        {
            if (CheckIfBlockHit())
            {
                hasHitBlock = true;
                this.SetWalkSpeed();
            }

        }
        base.StateUpdate();
    }

    public sealed override void OnStateEnter() 
    {
        KillSpeed();
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    #region Logic Functions

    public void UpdatePosition()
    {
        sprite.transform.position = new Vector3(
            sprite.transform.position.x + (sprite.speed * Time.deltaTime),
            sprite.transform.position.y + (sprite.yMoveDir * Time.deltaTime),
            sprite.transform.position.z);
    }

    public void UpdateRectAndCheckForCollisions()
    {
        sprite.rect.position = sprite.transform.position;

        sprite.CheckForBlockCollisions();

        sprite.rect.position = sprite.transform.position;
    }

    void KillSpeed()
    {
        sprite.xMoveDir = 0;
        sprite.speed = 0;
    }

    bool CheckIfBlockHit()
    {
        return (sprite.groundState is GroundedState);
    }
    void SetWalkSpeed()
    {
        sprite.xMoveDir = 1;
        sprite.speed = 5.0f;
    }

    #endregion
}
