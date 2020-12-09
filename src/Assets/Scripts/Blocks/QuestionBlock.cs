using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : Block
{
    bool struck = false;
    protected sealed override void Awake()
    {
        base.Awake();
    }

    #region Player Collision event
    public sealed override void HitBottom() 
    {
        if(!struck)
        {
            EmptyBlock();
            Bounce();
        }
        
    }

    void Bounce()
    {
        //Todo make blocks bounce up and down briefly when hit
    }

    void EmptyBlock()
    {
        //Todo add particle effect

        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blocks/Empty");
        ScoreService.Score += 100;
    }
    #endregion
}
