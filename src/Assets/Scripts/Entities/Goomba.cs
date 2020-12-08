using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Entity
{
    public State moveState;
    public State groundState;
    public State gameState;

    SpriteRenderer spriteRenderer;

    public float maxSpeed = 5;
    public float maxFallSpeed = 5;
    public float gravityStrength = 3;
    public override void Initialize(List<BlockChunk> chunks, List<Entity> entities)
    {
        base.Initialize(chunks, entities);

        this.moveState = new GoombaMoveState(this);
        this.groundState = new GoombaGroundState(this);

        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.speed = -maxSpeed;
    }

    void Update()
    {
        moveState.StateUpdate();
        groundState.StateUpdate();

        UpdateSpriteDirection();
        CheckIfOutOfBounds();
    }

    public override void HitTop(Block b)
    {
        this.transform.position = new Vector3(
        this.transform.position.x,
        b.rect.position.y + (b.rect.height / 2.0f) + (rect.height / 2.0f),
        this.transform.position.z);

        Land();
        
    }

    public override void HitSide(Block b, float dir)
    {
        //Kill speed
        this.speed *= -1;

        //We hit the left side of the block
        if (dir < 0)
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
        if (!(this.groundState is FallingState))
        {
            //Kill momentum
            this.yMoveDir = -1;

            this.transform.position = new Vector3(
                this.transform.position.x,
                b.rect.position.y - (b.rect.height / 2.0f) - (rect.height / 2.0f),
                this.transform.position.z);
        }
    }

    public override void HitSideEntity(Entity e)
    {
        if (e is Goomba)
            this.speed *= -1;
    }

    public override void CheckForFall()
    {
        if (this.blockTouchCount == 0)
            this.SetState(ref groundState, new GoombaFallingState(this));
    }

    void UpdateSpriteDirection()
    {
            if (this.spriteRenderer.flipX == true)
                this.spriteRenderer.flipX = false;
            else 
                this.spriteRenderer.flipX = true;
    }

    void Land()
    {
        this.SetState(ref this.groundState, new GoombaGroundState(this));
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    void CheckIfOutOfBounds()
    {
        if (this.rect.position.x < Utility.botLeft.x + (this.rect.width / 2.0f))
        {
            this.transform.position = new Vector3(
                Utility.botLeft.x + (this.rect.width / 2.0f),
                this.transform.position.y,
                0);

            this.rect = new Rect(this.transform.position, this.rect.size);

            this.speed = 0;
        }

        else if (this.rect.position.x > Utility.topRight.x - (this.rect.width / 2.0f))
        {
            this.transform.position = new Vector3(
                Utility.topRight.x - (this.rect.width / 2.0f),
                this.transform.position.y,
                0);

            this.rect = new Rect(this.transform.position, this.rect.size);

            this.speed = 0;
        }

        if (this.rect.position.y < Utility.botLeft.y - (this.rect.height / 2.0f))
            this.Die();
    }
}
