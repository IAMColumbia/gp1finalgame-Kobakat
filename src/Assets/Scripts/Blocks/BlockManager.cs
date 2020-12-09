using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public List<Block> blocks;
    public List<BlockChunk> chunks;

    public int chunkCount = 10;
    public float chunkPadding = 20; //Chunks are defined to the highest block point, of which the player may jump above. Add some padding to keep the player in a chunk 
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
    
    Texture2D map;
    Vector2 levelDimensions;
    Vector2 blockDimensions;
    Color[] colorLine;
    Color[,] colorMap;

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
        ParentBlocksToChunks();
    }

    #region Logic functions
    /// <summary>
    /// Initialize values from the level
    /// </summary>
    /// <param name="Map">The texture representing the level to load</param>
    void Initialize(Texture2D Map)
    {
        this.blockDimensions = Utility.blockIdentity / Utility.pixelsPerUnit;
        this.map = Map;
        this.levelDimensions = new Vector2(map.width, map.height);
        
        this.colorLine = map.GetPixels();
        this.colorMap = new Color[(int)levelDimensions.x, (int)levelDimensions.y];
    }

    /// <summary>
    /// Generate a collection of chunks. This makes entities only have to check for collisions
    /// with blocks that reside in the same chunk as them
    /// </summary>
    void GenerateChunks()
    {
        float height = levelDimensions.y * (Utility.blockIdentity.y / Utility.pixelsPerUnit) + chunkPadding;
        float width = levelDimensions.x * Utility.blockIdentity.x / (Utility.pixelsPerUnit * chunkCount);
        float yPos = (Utility.topRight.y - Utility.botLeft.y) / 2.0f;
        float xPos;

        //Makes chunks fit nicely to the blocks. Adds the cut off portion to the final chunk.
        float chunkBuffer = Mathf.Abs((width % (Utility.blockIdentity.x / Utility.pixelsPerUnit)));
        float finalChunkBuffer = 0;

        //Make the width ever so slightly less so it doesn't include neighboring chunk blocks
        width -= chunkBuffer - Utility.epsilon;

        for (int i = 0; i < chunkCount; i++)
        {
            finalChunkBuffer += chunkBuffer;
            
            xPos = (i * width) + (width / 2.0f) + Utility.botLeft.x;
            
            if (i == chunkCount - 1)
            {
                xPos = (i * width) + (width / 2.0f) + Utility.botLeft.x + (finalChunkBuffer / 2.0f);
                width += finalChunkBuffer;
            }
                
            BlockChunk newChunk = Instantiate(
                chunk, 
                new Vector3(xPos, yPos, 0), 
                this.transform.rotation, 
                this.transform).GetComponent<BlockChunk>();

            newChunk.Initialize(width, height);
            chunks.Add(newChunk);
        }
    }   

    void ParentBlocksToChunks()
    {
        foreach(BlockChunk c in this.chunks)
        {
            foreach(Block b in this.blocks)
            {
                if(Utility.Intersectcs(c.rect, b.rect))
                {
                    c.blocks.Add(b);
                    b.transform.parent = c.transform;
                }
            }
        }
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

                //Black place ground block
                if(hexColor == Utility.colorDictionary["Black"])
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
                    obj = Instantiate(fireBar, placeLocation, this.transform.rotation, this.transform);
                                    
                //Pipe
                else if(hexColor == Utility.colorDictionary["PipeTopL"])
                    obj = Instantiate(pipeTopL, placeLocation, this.transform.rotation, this.transform);

                else if (hexColor == Utility.colorDictionary["PipeTopR"])
                    obj = Instantiate(pipeTopR, placeLocation, this.transform.rotation, this.transform);

                else if (hexColor == Utility.colorDictionary["PipeL"])
                    obj = Instantiate(pipeL, placeLocation, this.transform.rotation, this.transform);

                else if (hexColor == Utility.colorDictionary["PipeR"])
                    obj = Instantiate(pipeR, placeLocation, this.transform.rotation, this.transform);

                //Any block
                if (obj && obj.GetComponent<Block>())
                    blocks.Add(obj.GetComponent<Block>());                                
            }
        }
    }

    void RemoveAllBlocks()
    {
        this.blocks.Clear();
        
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
        
    }

    #endregion
}
