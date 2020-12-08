using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag
{
    protected sealed override void Awake()
    {
        //Do nothing
    }

    #region Player Collision event

    public sealed override void HitTop() 
    {
        //TODO extra live
    }
    public sealed override void HitSide() { }
    public sealed override void HitBottom() { }

    void Initialize()
    {
       
    }
    #endregion

}
