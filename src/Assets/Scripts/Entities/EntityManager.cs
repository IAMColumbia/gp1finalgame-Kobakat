using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityManager
{
    List<Entity> entities { get; set; }
    List<BlockChunk> chunks { get; set; }
    Player player { get; set; }
    Rect goal { get; set; }
}

public class EntityManager : IEntityManager, IMappable
{
    public List<Entity> entities { get; set; }
    public List<BlockChunk> chunks { get; set; }
    public Player player { get; set; }
    
    public Texture2D map { get; set; }
    public Vector2 levelDimensions { get; set; }
    public Vector2 blockDimensions { get; set; }
    public Color[] colorLine { get; set; }
    public Color[,] colorMap { get; set; }

    public Rect goal { get; set; }

    public EntityManager(Texture2D Map, List<BlockChunk> Chunks)
    {
        this.entities = new List<Entity>();
        
        Initialize(Map, Chunks);
    }

    /// <summary>
    /// Initialize values from the level
    /// </summary>
    /// <param name="Map">The texture representing the level to load</param>
    void Initialize(Texture2D Map, List<BlockChunk> Chunks)
    {
        this.chunks = Chunks;
        this.map = Map;

        this.levelDimensions = new Vector2(map.width, map.height);
        this.blockDimensions = Utility.blockIdentity / Utility.pixelsPerUnit;
              
        this.colorMap = new Color[(int)levelDimensions.x, (int)levelDimensions.y];
           
    }
}
