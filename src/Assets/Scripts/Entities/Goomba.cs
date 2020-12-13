using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : UnityMover
{
    public State moveState;
    public State groundState;

    public float maxSpeed = 5;
    public float maxFallSpeed = 5;
    public float gravityStrength = 3;
    public override void Initialize(List<BlockChunk> Chunks, List<Entity> Entities, List<UnityBlockChunk> UnityChunks, List<UnityEntity> UnityEntities)
    {
        base.Initialize(Chunks, Entities, UnityChunks, UnityEntities);

        this.moveState = new GoombaMoveState(this);
        this.groundState = new GoombaGroundState(this);
      
        mover.speed = -maxSpeed;        
    }

    void Update()
    {
        CheckIfOutOfBounds();

        moveState.StateUpdate();
        groundState.StateUpdate();
     
        CheckForFall();
    }

    public override void HitTop(UnityBlock b)
    {
        base.HitTop(b);
        Land();     
    }

    public override void HitSide(UnityBlock b, float side)
    {
        base.HitSide(b, side);
        mover.speed *= -1;      
    }

    public override void HitBottom(UnityBlock b)
    {
        if (!(this.groundState is FallingState))
        {
            base.HitBottom(b);
 
            mover.yMoveDir = -1;          
        }
    }

    public override void HitTop(UnityEntity e)
    {
        if (e is UnityMover && !(e is Player)) 
            base.HitTop(e);
    }

    public override void HitSide(UnityEntity e, float side)
    {
        //Collide with all movers except the player
        if(e is UnityMover && !(e is Player))
        {
            base.HitSide(e, side);
            mover.speed *= -1;
        }           
    }

    public override void HitBottom(UnityEntity e)
    {
        //DoNothing
    }

    public void CheckIfOutOfBounds()
    {
        if (mover.WentOutOfBoundsSideways())
            mover.speed *= -1;
        if (mover.WentOutOfBoundsDown())
            this.Die();
    }

    public void Die()
    {
        this.gameObject.SetActive(false);
    }

    void Land()
    {
        mover.SetState(ref this.groundState, new GoombaGroundState(this));
    }

    void CheckForFall()
    {
        if (mover.CheckForFall())
        {           
            mover.SetState(ref this.groundState, new GoombaFallingState(this));
        }
    }

    #region Debug
    void OnDrawGizmos()
    {
        // Green
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
        DrawRect(entity.rect);
    }

    void OnDrawGizmosSelected()
    {
        // Orange
        Gizmos.color = new Color(1.0f, 0.5f, 0.0f);
        DrawRect(entity.rect);
    }

    void DrawRect(Rect rect)
    {
        Gizmos.DrawWireCube(rect.position, new Vector3(rect.width, rect.height, 0));
    }
    #endregion

}
