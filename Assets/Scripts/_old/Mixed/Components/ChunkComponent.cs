using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ChunkComponent : IComponentData
{
    // public int2 position;
    public float randomFloat;
    public int2 anchorCoordinats;
    public int2 coordinats;
    public int offset;
    public int size;
}

// public struct ChunkStateComponent : IComponentData
// {
//     public enum State
//     {
//         None,
//         Loaded,
//         Unloaded,
//     }

//     public State state;
// }

public struct ChunkLoadComponent : IComponentData { }
public struct ChunkCreateComponent : IComponentData { }
public struct UnloadChunkComponent : IComponentData { }


public struct ReqestChunkBuildComponent : IComponentData { }

public struct ChunkBuiltComponent : IComponentData { }


public struct ChunkLoadedComponent : IComponentData { }

public struct ChunkUnloadComponent : IComponentData { }
public struct ChunkUnloadedComponent : IComponentData { }

public struct ChunkDestryComponent : IComponentData { }

public struct ChunkHasChanged : IComponentData { }
public struct ChunkShouldRender : IComponentData { }

public struct TileBufferElement : IBufferElementData
{
    public int2 tile;
    // public Entity tileEntity;
}

