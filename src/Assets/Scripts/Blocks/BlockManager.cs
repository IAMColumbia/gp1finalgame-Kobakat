using System;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockManager
{
    List<Block> blocks { get; set; }
    List<BlockChunk> chunks { get; set; }

    int chunkCount { get; set; }
    float chunkPadding { get; set; }
}

public class BlockManager : IBlockManager, IMappable
{
    public List<Block> blocks { get; set; }
    public List<BlockChunk> chunks { get; set; }

    public int chunkCount { get; set; }
    public float chunkPadding { get; set; } //Chunks are defined to the highest block point, of which the player may jump above. Add some padding to keep the player in a chunk 
       
    public Texture2D map { get; set; }
    public Vector2 levelDimensions { get; set; }
    public Vector2 blockDimensions { get; set; }
    public Color[] colorLine { get; set; }
    public Color[,] colorMap { get; set; }

    public BlockManager(Texture2D Map)
    {
        Initialize(Map);
    }

    /// <summary>
    /// Initialize values from the level
    /// </summary>
    /// <param name="Map">The texture representing the level to load</param>
    void Initialize(Texture2D Map)
    {
        this.blocks = new List<Block>();
        this.chunks = new List<BlockChunk>();

        this.map = Map;

        this.levelDimensions = new Vector2(map.width, map.height);
        this.blockDimensions = Utility.blockIdentity / Utility.pixelsPerUnit;

        this.colorMap = new Color[(int)levelDimensions.x, (int)levelDimensions.y];
    }

    
}
