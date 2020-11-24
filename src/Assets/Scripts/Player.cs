using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Properties
    public List<Block> blocks;
    public State moveState;
    public State groundState;
    public Rect rect;

    //Hide these values from the inspector
    public bool hasDoubleJumped { get; set; }
    public float speed { get; set; }
    public float xMoveDir { get; set; }
    public float yMoveDir { get; set; }

    public float acceleration = 5;
    public float maxSpeed = 5;
    public float frictionStrength = 5;
    public float jumpStrength = 50;
    public float gravityStrength = 3;
    public float maxFallSpeed = 5;
    public float airborneMovementDamp = 50;
    
    float blockTouchCount;
    
    SpriteRenderer spriteRenderer;
    Sprite sprite;
    
    #endregion

    #region Unity Message Functions
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        sprite = this.spriteRenderer.sprite;
        rect = new Rect(this.transform.position, new Vector2(((float)this.sprite.texture.width / 100.0f), ((float)this.sprite.texture.height / 100.0f)));
        blocks = FindObjectOfType<Level>().GetComponent<Level>().blocks;

        this.moveState = new MovementState(this);
        this.groundState = new GroundedState(this);
    }

    void Update()
    {
        moveState.StateUpdate();
        groundState.StateUpdate();
    }
    #endregion

    #region Logic Functions


    public void CheckForBlockCollisions()
    {
        Block b = null;
        Vector2 vec = Vector2.zero;
        float dst = 10;
        blockTouchCount = 0;

        foreach(Block block in blocks)
        {        
            if(RectOverlap(this.rect, block.rect))
            {
                Vector2 newVec = rect.position - block.rect.position;

                float newDst = newVec.sqrMagnitude;

                if (newDst < dst)
                {
                    vec = newVec;
                    dst = newDst;
                    b = block;
                }

                blockTouchCount++;                     
            }
        }
        
        if(blockTouchCount > 0)
        {
            float angle = Vector2.Angle(vec, Vector2.up);

            if (angle < 42.0f)
                HitTop(b);
            else if (angle >= 42.0f && angle <= 135.0f)
                HitSide(b, vec.x);
            else
                HitBottom(b);

            Debug.DrawRay(b.transform.position, vec);
        }
        
        if (blockTouchCount == 0 && !(this.groundState is JumpingState))
            this.SetState(ref groundState, new FallingState(this));
    }

    void HitTop(Block b)
    {
        if(!(this.groundState is JumpingState))
        {
            this.transform.position = new Vector3(
            this.transform.position.x,
            b.rect.position.y + (b.rect.height / 2.0f) + (rect.height / 2.0f),
            this.transform.position.z);

            Land();
        }    
    }

    void HitSide(Block b, float dir)
    {
        //Kill speed
        this.speed = 0;

        //We hit the left side of the block
        if(dir < 0)
        {
            this.transform.position = new Vector3(
                b.rect.position.x - (b.rect.width / 2.0f) - (rect.width / 2.0f),
                this.transform.position.y,
                this.transform.position.z);
        }

        //We hit the right side of the block
        else
        {
            this.transform.position = new Vector3(
                b.rect.position.x + (b.rect.width / 2.0f) + (rect.width / 2.0f),
                this.transform.position.y,
                this.transform.position.z);
        }     
    }

    void HitBottom(Block b)
    {
        if(!(this.groundState is FallingState))
        {
            //Kill momentum
            this.yMoveDir = -1;

            this.transform.position = new Vector3(
                this.transform.position.x,
                b.rect.position.y - (b.rect.height / 2.0f) - (rect.height / 2.0f),
                this.transform.position.z);
        }    
    }

    void Land()
    {
        this.SetState(ref this.groundState, new GroundedState(this));
        this.hasDoubleJumped = false;
    }

    public void UpdateSpriteDirection()
    {
        if (this.xMoveDir > 0)
            this.spriteRenderer.flipX = false;
        else if (this.xMoveDir < 0)
            this.spriteRenderer.flipX = true;
    }

    bool RectOverlap(Rect a, Rect b)
    {
        if (
            a.x + (a.width / 2.0f) < b.x - (b.width / 2.0f)
            || a.y + (a.height / 2.0f) < b.y - (b.height / 2.0f)
            || b.x + (b.width / 2.0f) < a.x - (a.width / 2.0f)        
            || b.y + (b.height / 2.0f) < a.y - (a.height / 2.0f))
        {
            return false;
        }
       
        return true;
    }

    public void SetState(ref State whichState, State type)
    {
        whichState.OnStateExit();
        whichState = type;
        whichState.OnStateEnter();
    }

    #endregion

    #region Debug
    void OnDrawGizmos()
    {
        // Green
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
        DrawRect(rect);
    }

    void OnDrawGizmosSelected()
    {
        // Orange
        Gizmos.color = new Color(1.0f, 0.5f, 0.0f);
        DrawRect(rect);
    }

    void DrawRect(Rect rect)
    {
        Gizmos.DrawWireCube(rect.position, new Vector3(rect.width, rect.height, 0));
    }
    #endregion
}

