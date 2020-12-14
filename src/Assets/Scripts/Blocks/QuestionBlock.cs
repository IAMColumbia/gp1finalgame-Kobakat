using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : UnityBlock
{
    public bool struck = false;

    public float bumpSpeed = 10;
    public float maxHeight = 0.1f;

    bool isHit = false;
    bool isReturning = false;

    float initialHeight;
    public sealed override void Initialize()
    {
        base.Initialize();
        this.initialHeight = this.block.position.y;
    }

    #region Player Collision event
      
    void Update()
    {
        if (isHit)
        {
            this.block.position = new Vector3(
                this.block.position.x,
                this.block.position.y + (bumpSpeed * Time.deltaTime),
                this.block.position.z);

            if (this.block.position.y >= initialHeight + maxHeight)
            {
                this.isHit = false;
                this.isReturning = true;
            }
        }

        if (isReturning)
        {
            this.block.position = new Vector3(
                this.block.position.x,
                this.block.position.y - (bumpSpeed * Time.deltaTime),
                this.block.position.z);

            if (this.block.position.y <= initialHeight)
            {
                this.block.position = new Vector3(
                    this.block.position.x,
                    this.initialHeight,
                    this.block.position.z);

                this.isReturning = false;
            }
        }

        this.transform.position = this.block.position;
    }

    public sealed override void HitBottom()
    {
        if (!struck)
        {
            EmptyBlock();
            this.struck = true;
            this.isHit = true;
        }
    }

    void EmptyBlock()
    {
        //Todo add particle effect

        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blocks/Empty");
        ScoreService.Score += 100;
    }
    #endregion
}
