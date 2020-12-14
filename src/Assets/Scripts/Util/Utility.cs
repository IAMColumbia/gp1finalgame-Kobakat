using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Container Class holding logic functions/properties revolvng around game space
/// and rectangle collisions
/// </summary>

public class Utility : MonoBehaviour
{
    public static Vector3 botLeft;
    public static Vector3 topRight;

    public static float camHeight;
    public static float camWidth;

    //Benefit - One variable to control game "scale"
    //Drawback - this forces the entire project to use one conversion rate
    public static float pixelsPerUnit = 100.0f;

    //The dimensions of a block Identity, in pixels.
    //Hacky solution but ensures any prefab can change without ruining the game
    public static Vector2 blockIdentity = new Vector2(128, 128);

    /// Helps with floating point precision issues
    public static float epsilon { get; private set; } = 0.001f;

    Vector2 levelDimensions;

    #region Instance Setup
    public void Initialize(Texture2D Map)
    {   
        SetScreenBounds(Map);

        colorDictionary = new Dictionary<string, string>();
        this.FillDictionary();
    }

    void SetScreenBounds(Texture2D Map)
    {
        this.levelDimensions = new Vector2(Map.width, Map.height);

        camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        camHeight = Camera.main.orthographicSize;

        botLeft = new Vector3(-camWidth, -camHeight, 0);

        //We need the size of a block and the number of blocks to determine what X level the level ends at
        float rightBound = levelDimensions.x * blockIdentity.x / pixelsPerUnit;

        topRight = new Vector3(
            botLeft.x + rightBound,
            levelDimensions.y * (blockIdentity.y / pixelsPerUnit) + botLeft.y,
            0);

    }

    #endregion

    #region Rect Math
    /// <summary>
    /// Unity's Rect.Overlaps function does not work? No idea why
    /// Had to write my own, probably not as efficent. oh well.
    /// </summary>
    /// <param name="a">The first Rectangle</param>
    /// <param name="b">The rectangle to check collisions(overlaps) with</param>
    /// <returns></returns>
    public static bool Intersectcs(Rect a, Rect b)
    {
        if (
            a.x + (a.width / 2.0f) - epsilon < b.x - (b.width / 2.0f)
            || a.y + (a.height / 2.0f) - epsilon < b.y - (b.height / 2.0f)
            || b.x + (b.width / 2.0f) + epsilon < a.x - (a.width / 2.0f)
            || b.y + (b.height / 2.0f) + epsilon < a.y - (a.height / 2.0f))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Epsilon can ruin chunk calculations so this version removes it
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool ChunkIntersect(Rect a, Rect b)
    {
        if (
            a.x + (a.width / 2.0f) < b.x - (b.width / 2.0f)
            || a.y + (a.height / 2.0f) < b.y - (b.height / 2.0f)
            || b.x + (b.width / 2.0f) < a.x - (a.width / 2.0f)
            || b.y + (b.height / 2.0f) < a.y - (a.height / 2.0f))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Returns the minimum angle (in degrees) that two rects can collide with one another
    /// Before they are determined to be colliding on the sides instead of top/bottom
    /// </summary>
    /// <param name="a">The first rectangle</param>
    /// <param name="b">The second rectangle we are checking collisions(overlaps) with</param>
    /// <returns></returns>
    public static float MinDropAngle(Rect a, Rect b)
    {
        //Inverse tangent of 1/2 the added sprites height & width
        //returns the angle from a vector drawn center to center when
        //the rects are corner to corner

        float minAngle = Mathf.Atan(
                ((a.height / 2.0f) + (b.height / 2.0f)) /
                ((a.width / 2.0f) + (b.width / 2.0f)));

        //Convert that angle to degrees
        minAngle *= Mathf.Rad2Deg;

        return minAngle;
    }
    #endregion

    #region Color Dictionary

    public static Dictionary<string, string> colorDictionary;

    void FillDictionary()
    {      
        //Boring Blocks
        colorDictionary.Add("Black", "000000"); //Ground block
        colorDictionary.Add("Gray", "808080"); //Solid block
        
        //Interesting Blocks
        colorDictionary.Add("Steel", "404040"); //Fire bar
        colorDictionary.Add("Yellow", "FFFF00"); //Question Block
        colorDictionary.Add("Green", "00FF00"); //Flag pole
        //BreakableBlock
        colorDictionary.Add("Blue", "0000FF");

        //Entities
        colorDictionary.Add("Red", "FF0000"); //Mario
        colorDictionary.Add("Brown", "331900"); //Goomba
        colorDictionary.Add("Gold", "999900"); //Coin

        //Pipe blocks
        //This can be changed later to have an algorithm determine what pipe blocks should be what
        //For now the lever designers can suffer
        colorDictionary.Add("PipeTopL", "00CC00");
        colorDictionary.Add("PipeTopR", "33FF33");
        colorDictionary.Add("PipeL", "66FF66");
        colorDictionary.Add("PipeR", "99FF99");
    }
    #endregion
}
