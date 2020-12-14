using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnityMover : UnityEntity
{
    public Mover mover;

    public List<UnityBlockChunk> unityChunks { get; set; }
    public List<UnityBlockChunk> unityBlockChunksCurrentlyIn;
    public List<UnityEntity> unityEntities { get; set; }

    public Animator anim;

    public virtual void Initialize(List<BlockChunk> Chunks, List<Entity> Entities, List<UnityBlockChunk> UnityChunks, List<UnityEntity> UnityEntities)
    {
        base.Initialize();
        mover = new Mover(this.entity, Chunks, Entities);

        this.unityChunks = UnityChunks;
        this.unityBlockChunksCurrentlyIn = new List<UnityBlockChunk>();
        this.unityEntities = UnityEntities;

        this.anim = GetComponent<Animator>();
    }

    //Block collisions
    public virtual void HitTop(UnityBlock b)                 { mover.HitTop(b.block); }
    public virtual void HitSide(UnityBlock b, float side)    { mover.HitSide(b.block, side); }
    public virtual void HitBottom(UnityBlock b)              { mover.HitBottom(b.block); }

    //Entity collisions
    public virtual void HitTop(UnityEntity e)                { mover.HitTop(e.entity); }
    public virtual void HitSide(UnityEntity e, float side)   { mover.HitSide(e.entity, side); } 
    public virtual void HitBottom(UnityEntity e)             { mover.HitBottom(e.entity); }

}
