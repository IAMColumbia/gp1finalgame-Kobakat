using System;
using System.Collections.Generic;
using UnityEngine;

public interface ILevel
{
    Texture2D[] maps { get; set; }
}

public class Level : ILevel
{  
    public Texture2D[] maps { get; set; }

    public Level(Texture2D[] Maps)
    {
        this.maps = Maps;
    }
}
