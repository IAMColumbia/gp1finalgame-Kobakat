using System;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    #region Properties
    public Vector2 LevelDimensions { get; private set; }
    public List<Block> blocks { get; private set; }
    
    public GameObject blockPrefab;
    public GameObject playerPrefab;

    public Transform playerTransform { get; private set; }

    [SerializeField] GameObject utilityPrefab;
    [SerializeField] Texture2D[] maps = null;
    
    Block blockComponent = null;
    Utility utilityComponent = null;
    Texture2D currentMap;

    Color[] colorMap;
    Color[,] placeMap;

    #endregion

    #region Unity Event Functions
    void Awake()
    {
        SetUpComponents();
        LoadLevel(0);
    }
    #endregion

    #region Logic Functions

    public void SetUpComponents()
    {
        if (!blockPrefab || !playerPrefab || !utilityPrefab)
        {
            throw new System.Exception("Check that that level has all the required prefab instances assigned via inspector");
        }

        blockComponent = blockPrefab.GetComponent<Block>();
        blockComponent.sprite = blockPrefab.GetComponent<SpriteRenderer>().sprite;

        utilityComponent = Instantiate(utilityPrefab, this.transform).GetComponent<Utility>();

        blocks = new List<Block>();
    }

    /// <summary>
    /// Loads appropriate blocks into the scene
    /// </summary>
    public void LoadLevel(int levelIndex)
    {
        currentMap = maps[levelIndex];
        LevelDimensions = new Vector2(currentMap.width, currentMap.height);

        utilityComponent.SetScreenBounds();

        placeMap = new Color[(int)LevelDimensions.x, (int)LevelDimensions.y];
        colorMap = currentMap.GetPixels();

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
                    (Utility.botLeft.x + j * blockDimensions.x) + (blockDimensions.x / 2.0f),
                    (Utility.botLeft.y + i * blockDimensions.y) + (blockDimensions.y / 2.0f),
                    0);
                
                GameObject obj;

                if (placeMap[j, i] == Color.black)
                {
                    obj = Instantiate(blockPrefab, placeLocation, this.transform.rotation, this.transform);
                    blocks.Add(obj.GetComponent<Block>());
                }

                else if (placeMap[j, i] == Color.red)
                {
                    playerTransform = Instantiate(playerPrefab, placeLocation, this.transform.rotation, this.transform).transform;
                }
            }
        }
    }

    #endregion
}
