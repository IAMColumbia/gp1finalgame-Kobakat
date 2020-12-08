using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    #region Properties
    public State moveState;
    public State groundState;
    public State gameState;
    public Rect goalRect { get; set; }

    //Hide these values from the inspector
    public bool hasDoubleJumped { get; set; }

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
    public float winTime = 5;

    SpriteRenderer spriteRenderer = null;
    
    #endregion

    #region Unity Message Functions
    protected override void Awake()
    {
        base.Awake();
    }
    void OnEnable()
    {
        ScoreService.TimesUp += Die;
    }

    void OnDisable()
    {
        ScoreService.TimesUp -= Die;
    }

    public void Initialize(Rect goal)
    {       
        this.moveState = new MovementState(this);
        this.groundState = new GroundedState(this);
        this.gameState = new PlayState(this);
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.speed = 0.1f;

        this.goalRect = goal;       
    }

    void Update()
    {
        if(gameState is PlayState)
        {
            moveState.StateUpdate();
            groundState.StateUpdate();

            UpdateSpriteDirection();
            CheckIfOutOfBounds();

            CheckForGoalCollision();
        }     
        else if(gameState is WinState)
        {
            moveState.StateUpdate();
            groundState.StateUpdate();
            gameState.StateUpdate();
        }

        else
        {
            this.gameState.StateUpdate();
        }

    }
    #endregion

    #region Logic Functions

    public override void CheckForFall()
    {
        if (this.blockTouchCount == 0 
            && !(this.groundState is JumpingState) 
            && !(this.groundState is RisingState) 
            && !(this.groundState is SlidingState))
                this.SetState(ref groundState, new FallingState(this));
    }
    void CheckForGoalCollision()
    {
        if(Utility.Intersectcs(this.rect, goalRect))
        {
            Win();
        }
    }
    void UpdateSpriteDirection()
    {
        if (this.groundState is GroundedState && this.moveState is MovementState)
        {
            if (this.speed > 0)
                this.spriteRenderer.flipX = false;
            else if (this.speed < 0)
                this.spriteRenderer.flipX = true;
        }
    }

    void CheckIfOutOfBounds()
    {
        if(this.rect.position.x < Utility.botLeft.x + (this.rect.width / 2.0f))
        {
            this.transform.position = new Vector3(
                Utility.botLeft.x + (this.rect.width / 2.0f),
                this.transform.position.y,
                0);

            this.rect= new Rect(this.transform.position, this.rect.size);

            this.speed = 0;
        }

        else if(this.rect.position.x > Utility.topRight.x - (this.rect.width / 2.0f))
        {
            this.transform.position = new Vector3(
                Utility.topRight.x - (this.rect.width / 2.0f),
                this.transform.position.y,
                0);

            this.rect = new Rect(this.transform.position, this.rect.size);

            this.speed = 0;
        }
            
        if (this.rect.position.y < Utility.botLeft.y - (this.rect.height /2.0f))
            this.Die();
    }

    public override void HitTop(Block b)
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

    public override void HitSide(Block b, float dir)
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

    public override void HitBottom(Block b)
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

    public override void HitBottomEntity(Entity e)
    {
        if (e is Goomba)
            Die();
    }

    public override void HitSideEntity(Entity e)
    {
        if (e is Goomba)
            Die();
    }

    public override void HitTopEntity(Entity e)
    {
        if (e is Goomba)
        {
            Destroy(e.gameObject);

            this.yMoveDir = 15;
        }           
    }

    void Land()
    {
        this.SetState(ref this.groundState, new GroundedState(this));
        this.hasDoubleJumped = false;
    }

    void Die()
    {
        if(!(this.gameState is DyingState))
            this.SetState(ref this.gameState, new DyingState(this));
    }

    void Win()
    {
        this.SetState(ref this.gameState, new WinState(this));
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

