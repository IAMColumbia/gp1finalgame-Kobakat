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
    protected virtual void CheckForBlockCollisions(UnityMover m)
    {
        UnityBlock b = null;
        Vector2 vec = Vector2.zero;
        
        float dst = 10; //initialize this to an unimportant value

        m.mover.unitTouchCount = 0;
        m.mover.blockTouchCount = 0;
        foreach (UnityBlockChunk chunk in m.unityBlockChunksCurrentlyIn)
        {
            foreach(UnityBlock uBlock in chunk.unityBlocks)
            {
                if (Utility.Intersectcs(m.entity.rect, uBlock.block.rect) && m.gameObject.activeSelf)
                {
                    Vector2 newVec = m.entity.position - uBlock.block.position;

                    float newDst = newVec.magnitude;

                    if (newDst < dst)
                    {
                        vec = newVec;
                        dst = newDst;
                        b = uBlock;
                    }

                    m.mover.unitTouchCount++;
                    m.mover.blockTouchCount++;
                }
            }
        }

        if (m.mover.blockTouchCount > 0)
        {
            float minAngle = Utility.MinDropAngle(m.entity.rect, b.block.rect);

            minAngle = 90 - minAngle;

            float angle = Vector2.Angle(vec, Vector2.up);

            if (angle < minAngle)
                m.HitTop(b);
            else if (angle >= minAngle && angle <= 180 - minAngle)
                m.HitSide(b, vec.x);
            else
                m.HitBottom(b);

            Debug.DrawRay(b.block.rect.position, vec);
        }        
    }

    public virtual void CheckForChunksCurrentlyIn(UnityMover m)
    {
        m.unityBlockChunksCurrentlyIn.Clear(); 
        
        foreach(UnityBlockChunk c in m.unityChunks)
        {           
            if(Utility.Intersectcs(m.entity.rect, c.chunk.rect))
            {
                
                m.unityBlockChunksCurrentlyIn.Add(c);
               
                //Adds neighboring chunks into collision check. Maybe this can be used for something
                
                /*int index;
                index = m.unityChunks.IndexOf(c);

                if(m.unityChunks.IndexOf(c) != m.unityChunks.Count - 1)
                    m.unityBlockChunksCurrentlyIn.Add(m.unityChunks[index + 1]);

                if(m.unityChunks.IndexOf(c) != 0)
                    m.unityBlockChunksCurrentlyIn.Add(m.unityChunks[index - 1]);*/
            }
        }
    }
    public virtual void UpdateRectAndCheckForCollisions(UnityMover m)
    {
        m.entity.rect = new Rect(m.entity.position, m.entity.rect.size);

        CheckForBlockCollisions(m);

        m.entity.rect = new Rect(m.entity.position, m.entity.rect.size);
    }
    public virtual void UpdatePosition(UnityMover m)
    {
        m.entity.position = new Vector3(
            m.entity.position.x + (m.mover.speed * Time.deltaTime),
            m.entity.position.y + (m.mover.yMoveDir * Time.deltaTime),
            0);

        m.transform.position = m.entity.position;
    }

    public virtual void CheckForCollisionWithOtherEntities(UnityMover m)
    {
        Vector2 vec = Vector2.zero;
        float dst = 10;

        foreach (UnityEntity e in m.unityEntities)
        {
            if(m != e && e.gameObject.activeSelf)
            {
                if (Utility.Intersectcs(m.entity.rect, e.entity.rect))
                {
                    Vector2 newVec = m.entity.position - e.entity.position;

                    float newDst = newVec.magnitude;

                    if (newDst < dst)
                    {
                        vec = newVec;
                        dst = newDst;
                    }

                    float minAngle = Utility.MinDropAngle(m.entity.rect, e.entity.rect);

                    minAngle = 90 - minAngle;

                    float angle = Vector2.Angle(vec, Vector2.up);

                    if (angle < minAngle + 5)
                        m.HitTop(e);
                    else if (angle >= minAngle - 5 && angle <= 180 - minAngle)
                        m.HitSide(e, vec.x);
                    else
                        m.HitBottom(e);
                }
            }
            
        }
    }

    #endregion
}
