using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityBlockManager : MonoBehaviour
{
    public BlockManager manager = null;
    public List<UnityBlockChunk> unityBlockChunks { get; set; }
    public List<Fire> Fires; //Forgive me for I have sinned. Used for the fire bar blocks to pass the entities to the manager.

    List<UnityBlock> unityBlocks;
    
    [SerializeField] int chunkCount = 10;
    [SerializeField] float chunkPadding = 20;

    #region Block Prefab References
    [SerializeField] GameObject chunk = null;

    [SerializeField] GameObject groundBlock = null;
    [SerializeField] GameObject solidBlock = null;
    [SerializeField] GameObject brickBlock = null;
    [SerializeField] GameObject questionBlock = null;

    [SerializeField] GameObject pipeTopL = null;
    [SerializeField] GameObject pipeTopR = null;
    [SerializeField] GameObject pipeL = null;
    [SerializeField] GameObject pipeR = null;

    [SerializeField] GameObject fireBar = null;
    #endregion

    public void Initialize(Texture2D Map)
    {
        manager = new BlockManager(Map) { chunkCount = this.chunkCount, chunkPadding = this.chunkPadding };

        unityBlocks = new List<UnityBlock>();
        unityBlockChunks = new List<UnityBlockChunk>();
        Fires = new List<Fire>();
    }

    #region Build Level

    /// <summary>
    /// Removes and recreates a new level
    /// </summary>
    /// <param name="Map">The texture representing the level to load</param>
    public void BuildNewLevel(Texture2D Map)
    {
        RemoveAllBlocks();
        Initialize(Map);
        ConvertMapTo2DArray();
        PlaceBlockUsing2DMap();
        GenerateChunks();
        InitializeAllBlocks();
        ParentBlocksToChunks();
    }

    /// <summary>
    /// Generate a collection of chunks. This makes entities only have to check for collisions
    /// with blocks that reside in the same chunk as them
    /// </summary>
    void GenerateChunks()
    {
        float height = manager.levelDimensions.y * (Utility.blockIdentity.y / Utility.pixelsPerUnit) + this.chunkPadding;
        float width = manager.levelDimensions.x * Utility.blockIdentity.x / (Utility.pixelsPerUnit * this.chunkCount);
        float yPos = (Utility.topRight.y - Utility.botLeft.y) / 2.0f;
        float xPos;

        //Makes chunks fit nicely to the blocks. Adds the cut off portion to the final chunk.
        float chunkBuffer = Mathf.Abs((width % (Utility.blockIdentity.x / Utility.pixelsPerUnit)));
        float finalChunkBuffer = 0;

        //Make the width ever so slightly less so it doesn't include neighboring chunk blocks
        width -= chunkBuffer;

        for (int i = 0; i < chunkCount; i++)
        {
            finalChunkBuffer += chunkBuffer;

            xPos = (i * width) + (width / 2.0f) + Utility.botLeft.x;

            if (i == chunkCount - 1)
            {
                xPos = (i * width) + (width / 2.0f) + Utility.botLeft.x + (finalChunkBuffer / 2.0f);
                width += finalChunkBuffer;
            }

            UnityBlockChunk newChunk = Instantiate(
                chunk,
                new Vector3(xPos, yPos, 0),
                this.transform.rotation,
                this.transform).GetComponent<UnityBlockChunk>();

            newChunk.Initialize(width - Utility.epsilon, height);

            this.unityBlockChunks.Add(newChunk.GetComponent<UnityBlockChunk>());
            manager.chunks.Add(newChunk.GetComponent<UnityBlockChunk>().chunk);
            
        }
    }

    void ParentBlocksToChunks()
    {
        foreach (UnityBlockChunk c in this.unityBlockChunks)
        {
            foreach (UnityBlock b in this.unityBlocks)
            {
                if (Utility.ChunkIntersect(c.chunk.rect, b.block.rect))
                {
                    c.unityBlocks.Add(b);
                    c.chunk.blocks.Add(b.block);                  
                    b.transform.parent = c.transform;
                }
            }
        }
    }

    void InitializeAllBlocks()
    {
        foreach(UnityBlock b in this.unityBlocks)
        {
            //Fire blocks already intiialzied dont do it again
            if(!(b is FireBlock))
                b.Initialize();
        }
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

                //Black place ground block
                if (hexColor == Utility.colorDictionary["Black"])
                    obj = Instantiate(groundBlock, placeLocation, this.transform.rotation, this.transform);

                //Gray place solid block
                else if (hexColor == Utility.colorDictionary["Gray"])
                    obj = Instantiate(solidBlock, placeLocation, this.transform.rotation, this.transform);

                //Blue place brick block
                else if (hexColor == Utility.colorDictionary["Blue"])
                    obj = Instantiate(brickBlock, placeLocation, this.transform.rotation, this.transform);

                //Yellow place question block
                else if (hexColor == Utility.colorDictionary["Yellow"])
                    obj = Instantiate(questionBlock, placeLocation, this.transform.rotation, this.transform);

                //Steel place firebar
                else if (hexColor == Utility.colorDictionary["Steel"])
                {
                    obj = Instantiate(fireBar, placeLocation, this.transform.rotation, this.transform);
                    obj.GetComponent<UnityBlock>().Initialize();

                    foreach (Fire f in obj.GetComponent<FireBlock>().fires)
                        Fires.Add(f);
                }
                    
                    
                //Pipe
                else if (hexColor == Utility.colorDictionary["PipeTopL"])
                    obj = Instantiate(pipeTopL, placeLocation, this.transform.rotation, this.transform);

                else if (hexColor == Utility.colorDictionary["PipeTopR"])
                    obj = Instantiate(pipeTopR, placeLocation, this.transform.rotation, this.transform);

                else if (hexColor == Utility.colorDictionary["PipeL"])
                    obj = Instantiate(pipeL, placeLocation, this.transform.rotation, this.transform);

                else if (hexColor == Utility.colorDictionary["PipeR"])
                    obj = Instantiate(pipeR, placeLocation, this.transform.rotation, this.transform);

                //Any block
                if (obj && obj.GetComponent<UnityBlock>())
                {
                    this.unityBlocks.Add(obj.GetComponent<UnityBlock>());
                    manager.blocks.Add(obj.GetComponent<UnityBlock>().block);
                }
                       
            }
        }
    }

    void RemoveAllBlocks()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion
}
