using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Properties
    public List<Block> blocks;
    public State moveState;
    public State groundState;
    public State gameState;
    public Rect rect;

    //Hide these values from the inspector
    public bool hasDoubleJumped { get; set; }
    public float speed { get; set; }
    public float xMoveDir { get; set; }
    public float yMoveDir { get; set; }
    public float startLocation { get; set; }

    public float acceleration = 5;
    public float maxSpeed = 5;
    public float frictionStrength = 5;
    public float jumpBurstStrength = 10;
    public float jumpDeltaStrength = 0.25f;
    public float gravityStrength = 3;
    public float maxFallSpeed = 5;
    public float airborneMovementDamp = 50;
    public float deathTime = 3;
    
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
        this.gameState = new PlayState(this);
        this.speed = 0.1f;
    }

    void Update()
    {
        if(gameState is PlayState)
        {
            moveState.StateUpdate();
            groundState.StateUpdate();

            UpdateSpriteDirection();
            CheckIfOutOfBounds();
        }
        
        else
        {
            this.gameState.StateUpdate();
        }

        Debug.Log(moveState);
    }
    #endregion

    #region Logic Functions

    //If a sprite has an even number of pixels, each side of the block behaves differently
    //Add a small number to correct that

    public void CheckForBlockCollisions()
    {
        Block b = null;
        Vector2 vec = Vector2.zero;
        float dst = 10;
        blockTouchCount = 0;

        foreach(Block block in blocks)
        {        
            if(Utility.Intersectcs(this.rect, block.rect))
            {
                Vector2 newVec = rect.position - block.rect.position;

                float newDst = newVec.magnitude;

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
            float minAngle = Utility.MinDropAngle(this.rect, b.rect);

            minAngle = 90 - minAngle;

            float angle = Vector2.Angle(vec, Vector2.up);

            //Can/might be abstracted at some point
            if (angle < minAngle)
                HitTop(b);
            else if (angle >= minAngle && angle <= 180 - minAngle)
                HitSide(b, vec.x);
            else
                HitBottom(b);

            Debug.DrawRay(b.rect.position, vec);
        }
        
        if (blockTouchCount == 0 && !(this.groundState is JumpingState) && !(this.groundState is RisingState))
            this.SetState(ref groundState, new FallingState(this));
    }

    void CheckIfOutOfBounds()
    {
        if(this.rect.position.x < Utility.botLeft.x + (this.rect.width / 2.0f))
        {
            this.transform.position = new Vector3(
                Utility.botLeft.x + (this.rect.width / 2.0f),
                this.transform.position.y,
                0);

            this.rect.position = this.transform.position;

            this.speed = 0;
        }

        else if(this.rect.position.x > Utility.botRight.x - (this.rect.width / 2.0f))
        {
            this.transform.position = new Vector3(
                Utility.botRight.x - (this.rect.width / 2.0f),
                this.transform.position.y,
                0);

            this.rect.position = this.transform.position;

            this.speed = 0;
        }
            
        if (this.rect.position.y < Utility.botLeft.y - (this.rect.height /2.0f) && !(this.groundState is DyingState))
            this.Die();
    }

    void HitTop(Block b)
    {
        if (!(this.groundState is JumpingState) && !(this.groundState is RisingState))
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

    void Die()
    {
        this.SetState(ref this.gameState, new DyingState(this));
    }

    public void UpdateSpriteDirection()
    {
        if(this.groundState is GroundedState && this.moveState is MovementState)
        {
            if (this.speed > 0)
                this.spriteRenderer.flipX = false;
            else if (this.speed < 0)
                this.spriteRenderer.flipX = true;
        }     
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

