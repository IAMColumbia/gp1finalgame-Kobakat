using UnityEngine;

/// <summary>
/// Container Class holding logic functions/properties revolvng around game space
/// and rectangle collisions
/// </summary>

public class Utility : MonoBehaviour
{
    
    public static Vector3 botLeft;
    public static Vector3 botRight;

    public static float camHeight;
    public static float camWidth;
    
    Level level;

    /// Helps with floating point precision issues
    const float epsilon = 0.0001f;

    public void SetScreenBounds()
    {
        if (level == null)
            level = FindObjectOfType<Level>().GetComponent<Level>();

        camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        camHeight = Camera.main.orthographicSize;

        botLeft = new Vector3(-camWidth, -camHeight, 0);

        //We need the size of a block and the number of blocks to determine what X level the level ends at
        float rightBound =
            level.LevelDimensions.x *
            ((level.blockPrefab.GetComponent<SpriteRenderer>().sprite.texture.width / 100.0f)
            * level.blockPrefab.transform.localScale.x);
            

        botRight = new Vector3(
            botLeft.x + rightBound,
            -camHeight,
            0);

    }

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
}
