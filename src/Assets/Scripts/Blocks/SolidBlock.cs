using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Do nothing. This is just a plain block
public class SolidBlock : Block
{
    protected sealed override void Awake()
    {
        base.Awake();
    }

    #region Player Collision event
    //Solid Blocks don't do anything
    public sealed override void HitTop() { }
    public sealed override void HitSide() { }
    public sealed override void HitBottom() { }
    #endregion

}
