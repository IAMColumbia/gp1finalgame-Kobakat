using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveState: State, IMoveState
{
    

    #region State Events
    public override void OnStateEnter() { }


    public override void StateUpdate() { }

    public override void OnStateExit() { }
    #endregion

    #region State Logic
    protected virtual void CheckForBlockCollisions(Entity Entity)
    {
        Block b = null;
        Vector2 vec = Vector2.zero;
        float dst = 10;
        Entity.blockTouchCount = 0;

        foreach (BlockChunk chunk in Entity.chunksCurrentlyIn)
        {
            foreach(Block block in chunk.blocks)
            {
                if (Utility.Intersectcs(Entity.rect, block.rect))
                {
                    Vector2 newVec = new Vector2(Entity.transform.position.x, Entity.transform.position.y) - block.rect.position;

                    float newDst = newVec.magnitude;

                    if (newDst < dst)
                    {
                        vec = newVec;
                        dst = newDst;
                        b = block;
                    }

                    Entity.blockTouchCount++;
                }
            }
        }

        if (Entity.blockTouchCount > 0)
        {
            float minAngle = Utility.MinDropAngle(Entity.rect, b.rect);

            minAngle = 90 - minAngle;

            float angle = Vector2.Angle(vec, Vector2.up);

            if (angle < minAngle)
                Entity.HitTop(b);
            else if (angle >= minAngle && angle <= 180 - minAngle)
                Entity.HitSide(b, vec.x);
            else
                Entity.HitBottom(b);

            Debug.DrawRay(b.rect.position, vec);
        }
        
        Entity.CheckForFall();

    }

    public virtual void CheckForChunksCurrentlyIn(Entity Entity)
    {
        if(Entity.chunksCurrentlyIn.Count > 0)
        {
            Entity.chunksCurrentlyIn.Clear();
        }
        

        foreach(BlockChunk chunk in Entity.chunksToCheckCollisionFor)
        {
            
            if(Utility.Intersectcs(Entity.rect, chunk.rect))
            {

                Entity.chunksCurrentlyIn.Add(chunk);
            }
        }
    }
    public virtual void UpdateRectAndCheckForCollisions(Entity Entity)
    {
        Entity.rect = new Rect(Entity.transform.position, Entity.rect.size);

        CheckForBlockCollisions(Entity);

        Entity.rect = new Rect(Entity.transform.position, Entity.rect.size);
    }
    public virtual void UpdatePosition(Entity Entity)
    {
        Entity.transform.position = new Vector3(
            Entity.transform.position.x + (Entity.speed * Time.deltaTime),
            Entity.transform.position.y + (Entity.yMoveDir * Time.deltaTime),
            Entity.transform.position.z);
    }

    #endregion
}
