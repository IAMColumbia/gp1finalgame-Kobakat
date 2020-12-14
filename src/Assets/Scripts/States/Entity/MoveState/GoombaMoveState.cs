using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaMoveState : MoveState
{
    Goomba goomba;

    public GoombaMoveState(Goomba Goomba)
    {
        this.goomba = Goomba;
    }

    #region State Events
    public sealed override void StateUpdate()
    {
        base.CheckForChunksCurrentlyIn(goomba);;
        base.UpdatePosition(goomba);     
        base.UpdateRectAndCheckForCollisions(goomba);
        base.CheckForCollisionWithOtherEntities(goomba);

        goomba.entity.rect = new Rect(goomba.entity.position, goomba.entity.rect.size);
    }

    public sealed override void OnStateEnter() 
    {
        SetAnim();
        base.OnStateEnter(); 
    }
    public sealed override void OnStateExit() { base.OnStateExit(); }
    #endregion

    void SetAnim()
    {
        goomba.anim.Play(goomba.walkState);
    }
}
