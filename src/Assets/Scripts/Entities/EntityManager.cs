using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    List<Entity> entities;
    List<BlockChunk> chunks;

    public Rect goal;

    #region Entity Prefab References

    [SerializeField] GameObject mario = null;
    [SerializeField] GameObject goomba = null;

    [SerializeField] GameObject coin = null;
    [SerializeField] GameObject flag = null;

    Player playerComponent = null;
    #endregion

    Texture2D map;
    Vector2 levelDimensions;
    Vector2 blockDimensions;
    Color[] colorLine;
    Color[,] colorMap;

    /// <summary>
    /// Removes and recreates a new level
    /// </summary>
    /// <param name="Map">The texture representing the level to load</param>
    public void BuildNewLevel(Texture2D Map, List<BlockChunk> Chunks)
    {
        

        RemoveAllEntities();
        Initialize(Map, Chunks);
        ConvertMapTo2DArray();
        PlaceBlockUsing2DMap();
        InitializeAllEntities();

        Camera.main.GetComponent<CameraFollow>().target = playerComponent.transform;
    }

    #region Logic functions
    /// <summary>
    /// Initialize values from the level
    /// </summary>
    /// <param name="Map">The texture representing the level to load</param>
    void Initialize(Texture2D Map, List<BlockChunk> Chunks)
    {
        this.blockDimensions = Utility.blockIdentity / Utility.pixelsPerUnit;
        this.map = Map;
        this.levelDimensions = new Vector2(map.width, map.height);

        this.colorLine = map.GetPixels();
        this.colorMap = new Color[(int)levelDimensions.x, (int)levelDimensions.y];

        this.chunks = Chunks;      
    }

    /// <summary>
    /// Transform the 1D array produced by Unity's GetPixels method into a 2D array
    /// </summary>
    void ConvertMapTo2DArray()
    {
        colorLine = map.GetPixels();

        for (int i = 0; i < (int)levelDimensions.y; i++)
        {
            for (int j = 0; j < (int)levelDimensions.x; j++)
            {
                colorMap[j, i] = colorLine[i * (int)levelDimensions.x + j];
            }
        }
    }

    /// <summary>
    /// Read the level texture and place blocks accordingly
    /// </summary>
    void PlaceBlockUsing2DMap()
    {
        string hexColor;
        Vector3 placeLocation;

        for (int i = 0; i < (int)levelDimensions.y; i++)
        {
            for (int j = 0; j < (int)levelDimensions.x; j++)
            {
                placeLocation = new Vector3(
                    (Utility.botLeft.x + j * blockDimensions.x) + (blockDimensions.x / 2.0f),
                    (Utility.botLeft.y + i * blockDimensions.y) + (blockDimensions.y / 2.0f),
                    0);

                GameObject obj = null;
                hexColor = ColorUtility.ToHtmlStringRGB(colorMap[j, i]);

                //Red place mario
                if (hexColor == Utility.colorDictionary["Red"])
                {
                    obj = Instantiate(mario, placeLocation, this.transform.rotation, this.transform);
                    playerComponent = obj.GetComponent<Player>();
                }
                    

                //Brown place goomba
                else if (hexColor == Utility.colorDictionary["Brown"])
                    obj = Instantiate(goomba, placeLocation, this.transform.rotation, this.transform);

                //Gold place coin
                else if (hexColor == Utility.colorDictionary["Gold"])
                    obj = Instantiate(coin, placeLocation, this.transform.rotation, this.transform);

                //Green place flag pole
                else if (hexColor == Utility.colorDictionary["Green"])
                {
                    obj = Instantiate(flag, placeLocation, this.transform.rotation, this.transform);
                    goal = obj.GetComponentInChildren<Flag>().rect;
                    entities.Add(obj.GetComponentInChildren<Entity>());
                }
                    
                if(obj && obj.GetComponent<Entity>())
                {
                    entities.Add(obj.GetComponent<Entity>());
                }
                
            }
        }
    }

    void InitializeAllEntities()
    {
        foreach(Entity entity in this.entities)
        {
            if (!entity)
            {
                Debug.Log(entities.IndexOf(entity));
                Debug.Log(entity);
            }
           
            entity.Initialize(chunks, entities);
                     
        }

        playerComponent.Initialize(this.goal);
    }

    void RemoveAllEntities()
    {
        this.entities = new List<Entity>();

        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }      
    }

    #endregion
}
