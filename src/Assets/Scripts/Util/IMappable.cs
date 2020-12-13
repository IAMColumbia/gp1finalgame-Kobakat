using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMappable
{
    Texture2D map { get; set; }

    Vector2 levelDimensions { get; set; }
    Vector2 blockDimensions { get; set; }

    Color[] colorLine { get; set; }
    Color[,] colorMap { get; set; }
}
