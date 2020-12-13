using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityEntityManager : MonoBehaviour
{
    EntityManager manager = null;
    List<UnityEntity> unityEntities { get; set; }
    List<UnityBlockChunk> unityChunks { get; set; }
    #region Prefab Instances

    [SerializeField] GameObject mario;
    [SerializeField] GameObject goomba;
    [SerializeField] GameObject flag;
    [SerializeField] GameObject coin;

    #endregion

    public void Initialize(Texture2D Map, List<BlockChunk> Chunks, List<UnityBlockChunk> UnityChunks, List<Fire> Fires)
    {
        manager = new EntityManager(Map, Chunks);
        unityChunks = UnityChunks;
        unityEntities = new List<UnityEntity>();

        foreach (Fire f in Fires)
            unityEntities.Add(f);
    }

    #region Build Level
    /// <summary>
    /// Removes and recreates a new level
    /// </summary>
    /// <param name="Map">The texture representing the level to load</param>
    public void BuildNewLevel(Texture2D Map, List<BlockChunk> Chunks, List<UnityBlockChunk> UnityChunks, List<Fire> Fires)
    {
        RemoveAllEntities();
        Initialize(Map, Chunks, UnityChunks, Fires);
        ConvertMapTo2DArray();
        PlaceBlockUsing2DMap();
        InitializeAllEntities();

        Camera.main.GetComponent<CameraFollow>().target = manager.player.transform;
    }

    /// <summary>
    /// Transform the 1D array produced by Unity's GetPixels method into a 2D array
    /// </summary>
    void ConvertMapTo2DArray()
    {
        manager.colorLine = manager.map.GetPixels();

        for (int i = 0; i < (int)manager.levelDimensions.y; i++)
        {
            for (int j = 0; j < (int)manager.levelDimensions.x; j++)
            {
                manager.colorMap[j, i] = manager.colorLine[i * (int)manager.levelDimensions.x + j];
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

        for (int i = 0; i < (int)manager.levelDimensions.y; i++)
        {
            for (int j = 0; j < (int)manager.levelDimensions.x; j++)
            {
                placeLocation = new Vector3(
                    (Utility.botLeft.x + j * manager.blockDimensions.x) + (manager.blockDimensions.x / 2.0f),
                    (Utility.botLeft.y + i * manager.blockDimensions.y) + (manager.blockDimensions.y / 2.0f),
                    0);

                GameObject obj = null;
                hexColor = ColorUtility.ToHtmlStringRGB(manager.colorMap[j, i]);

                //Red place mario
                if (hexColor == Utility.colorDictionary["Red"])
                {
                    obj = Instantiate(mario, placeLocation, this.transform.rotation, this.transform);
                    manager.player = obj.GetComponent<Player>();
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
                    obj.GetComponentInChildren<Flag>().Initialize();
                    manager.goal = obj.GetComponentInChildren<Flag>().entity.rect;
                    manager.entities.Add(obj.GetComponentInChildren<UnityEntity>().entity);
                }

                if (obj && obj.GetComponent<UnityEntity>())
                {
                    manager.entities.Add(obj.GetComponent<UnityEntity>().entity);
                    unityEntities.Add(obj.GetComponent<UnityEntity>());
                }

            }
        }
    }

    void InitializeAllEntities()
    {
        foreach (UnityEntity entity in unityEntities)
        {
            //If its a mover cast it so it can initialize those components
            if(entity is UnityMover)
            {
                UnityMover mover = (UnityMover)entity;
                mover.Initialize(manager.chunks, manager.entities, unityChunks, unityEntities);
            }

            else
            {
                entity.Initialize();
            }           
        }
        manager.player.Initialize(manager.goal);

    }

    void RemoveAllEntities()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion
}
