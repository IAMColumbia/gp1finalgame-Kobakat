using System;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    #region Properties

    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject playerPrefab;

    [SerializeField] Texture2D[] maps = null;
    [SerializeField] Vector2 LevelDimensions = new Vector2(28, 10);

    public List<Block> blocks { get; private set; }
    Block blockComponent = null;

    Color[] colorMap;
    Color[,] placeMap;

    #endregion

    #region Unity Event Functions
    void Awake()
    {
        SetUpComponents();
        LoadLevel();
    }
    #endregion

    #region Logic Functions

    public void SetUpComponents()
    {
        placeMap = new Color[(int)LevelDimensions.x, (int)LevelDimensions.y];

        if (blockPrefab == null)
        {
            throw new System.Exception("The block manager needs a prefab instance of a block game object assigned in the inpsector");
        }

        blockComponent = blockPrefab.GetComponent<Block>();
        blockComponent.sprite = blockPrefab.GetComponent<SpriteRenderer>().sprite;

        blocks = new List<Block>();
    }

    /// <summary>
    /// Loads appropriate blocks into the scene
    /// </summary>
    public void LoadLevel()
    {
        colorMap = maps[0].GetPixels();

        ConvertMapTo2DArray();
        PlaceBlockUsing2DMap();
    }

    /// <summary>
    /// This function isn't necessary but I am much more comfortable thinking about this in 2 Dimensions over just 1
    /// </summary>
    void ConvertMapTo2DArray()
    {
        for (int i = 0; i < (int)LevelDimensions.y; i++)
        {
            for (int j = 0; j < (int)LevelDimensions.x; j++)
            {
                placeMap[j, i] = colorMap[i * (int)LevelDimensions.x + j];
            }
        }
    }

    /// <summary>
    /// Place blocks using the 2D color array
    /// This reads from bot left to top right
    /// </summary>

    void PlaceBlockUsing2DMap()
    {
        Vector3 placeLocation;

        Vector2 botleft = Camera.main.ScreenToWorldPoint(Vector3.zero);

        //TODO update pixel hard coded "100" to a pixel to unit conversion variable.
        Vector2 blockDimensions = new Vector2(
            blockComponent.sprite.texture.width * blockPrefab.transform.localScale.x,
            blockComponent.sprite.texture.height * blockPrefab.transform.localScale.y)
            / 100.0f;


        for (int i = 0; i < (int)LevelDimensions.y; i++)
        {
            for (int j = 0; j < (int)LevelDimensions.x; j++)
            {
                placeLocation = new Vector3(
                    botleft.x + blockDimensions.x + j * blockDimensions.x,
                    botleft.y + blockDimensions.y + i * blockDimensions.y,
                    0);

                GameObject obj;

                if (placeMap[j, i] == Color.black)
                {
                    obj = Instantiate(blockPrefab, placeLocation, this.transform.rotation, this.transform);
                    blocks.Add(obj.GetComponent<Block>());
                }

                else if (placeMap[j, i] == Color.red)
                {
                    Instantiate(playerPrefab, placeLocation, this.transform.rotation, this.transform);
                }
            }
        }
    }

    #endregion
}
