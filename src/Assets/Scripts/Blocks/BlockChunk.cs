using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockChunk
{
    List<Block> blocks { get; set; }
    Rect rect { get; set; }
    Vector3 position { get; set; }
}

public class BlockChunk : IBlockChunk
{
    public List<Block> blocks { get; set; }
    public Rect rect { get; set; }
    public Vector3 position { get; set; }

    public BlockChunk()
    {
        this.blocks = new List<Block>();
        this.rect = new Rect();
        this.position = Vector3.zero;
    }   
}
