using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBlock : UnityBlock
{
    public sealed override void Initialize()
    {
        base.Initialize();
    }

    #region Player Collision event

    public sealed override void HitBottom() 
    {
        Bump();
    }

    void Bump() { }
    #endregion

}
