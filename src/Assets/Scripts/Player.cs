using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnityMover
{
    #region Properties
    public State gameState;
    public State moveState;
    public State groundState;
    public Rect goalRect { get; set; }

    public float acceleration = 5;
    public float maxSpeed = 5;
    public float frictionStrength = 5;
    public float jumpBurstStrength = 10;
    public float jumpDeltaStrength = 0.25f;
    public float gravityStrength = 3;
    public float maxFallSpeed = 5;
    public float airborneMovementDamp = 1.05f;
    public float deathTime = 3;
    public float deathPauseTime = 1;
    public float winTime = 5;

    SpriteRenderer spriteRenderer = null;
    
    #endregion

    #region Unity Message Functions
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
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();

        this.gameState = new PlayState(this);
        this.moveState = new MovementState(this);
        this.groundState = new GroundedState(this);
              
        mover.speed = 0.1f;

        //Make sure mario is using the proper sprite and rect size
        this.spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/mario");

        this.goalRect = goal;       
    }

    void Update()
    {
        if(gameState is PlayState)
        {
            this.moveState.StateUpdate();
            this.groundState.StateUpdate();
          
            CheckIfOutOfBounds();
            CheckForGoalCollision();

            UpdateSpriteDirection();
            CheckForFall();
        }     
        else if(gameState is WinState)
        {
           this.moveState.StateUpdate();
           this.groundState.StateUpdate();
           this.gameState.StateUpdate();

           UpdateSpriteDirection();
           CheckForFall();
        }

        //Dying
        else
        {
            this.gameState.StateUpdate();
        }

    }
    #endregion

    #region Logic Functions

    void CheckForFall()
    {
        if (mover.CheckForFall())
        { 
            if(!(this.groundState is JumpingState)
            && !(this.groundState is RisingState)
            && !(this.groundState is SlidingState))
                mover.SetState(ref this.groundState, new FallingState(this));
        }
    }

    void CheckForGoalCollision()
    {
        if(Utility.Intersectcs(entity.rect, goalRect))
        {
            Win();
        }
    }
    void UpdateSpriteDirection()
    {
        if (!(this.moveState is TurningState))
        {
            if (mover.xMoveDir > 0)
                this.spriteRenderer.flipX = false;
            else if (mover.xMoveDir < 0)
                this.spriteRenderer.flipX = true;
        }
    }

    void CheckIfOutOfBounds()
    {
        if (mover.WentOutOfBoundsSideways())
            mover.speed = 0;
        if (mover.WentOutOfBoundsDown())
            this.Die();
    }

    public override void HitTop(UnityBlock b)
    {
        if (!(this.groundState is JumpingState) && !(this.groundState is RisingState))
        {
            base.HitTop(b);
            Land();
        }    
    }

    public override void HitSide(UnityBlock b, float side)
    {
        base.HitSide(b, side);
        mover.speed = 0;       
    }

    public override void HitBottom(UnityBlock b)
    {
        b.HitBottom();

        if(!(this.groundState is FallingState))
        {
            //Kill momentum
            mover.yMoveDir = -1;
            base.HitBottom(b);
        }    
    }

    public override void HitBottom(UnityEntity e)
    {
        if (e is Goomba)
            Die();

        if(e is Coin)
        {
            e.gameObject.SetActive(false);
            GetCoin();
        }
            
    }

    public override void HitSide(UnityEntity e, float side)
    {
        if (e is Goomba)
            Die();

        if (e is Coin)
        {
            e.gameObject.SetActive(false);
            GetCoin();
        }
    }
    public override void HitTop(UnityEntity e)
    {
        if (e is Goomba)
        {
            e.gameObject.SetActive(false);
            BounceOffGoomba();          
        }

        if (e is Coin)
        {
            e.gameObject.SetActive(false);
            GetCoin();
        }
    }

    void Land()
    {
        if(!(this.groundState is GroundedState))
            mover.SetState(ref this.groundState, new GroundedState(this));
    }

    void Die()
    {
        if(!(this.gameState is DyingState))
        {
            mover.SetState(ref this.gameState, new DyingState(this));
            this.spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/mariodead");
        }           
    }

    void Win()
    {
        mover.SetState(ref this.gameState, new WinState(this));
    }

    void BounceOffGoomba()
    {   
        //Todo play sound

        mover.yMoveDir = jumpBurstStrength;
        mover.SetState(ref groundState, new RisingState(this));
        ScoreService.Score += 100;
        
    }

    void GetCoin()
    {
        //Todo Play sound
        ScoreService.Score += 100;
    }
    #endregion

    #region Debug
    #if UNITY_EDITOR
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
    #endif
    #endregion
}

