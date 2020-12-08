using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBlock : Block
{
    protected sealed override void Awake()
    {
        base.Awake();
    }

    #region Player Collision event
    public sealed override void HitTop() { }
    public sealed override void HitSide() { }
    public sealed override void HitBottom() 
    {
        Destroy(this.gameObject);
    }
    #endregion

}
