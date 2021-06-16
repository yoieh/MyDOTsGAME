using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

// TODO: Redo chunk / tile calculations. view distance and more.

// To create a new chunk these steps is needed.
// 1. Create new chunk entity with ChunkComponent.
//    This needed to create tiles and render the mesh with textures.
//    Every tile needs to be abel to store its own data.
// 
//      tileData: Dont need to be synced from server
//          * coordinats, global int2 coordinat
//          * tileNose value, used to know what tile textere to use.
// 
//      Some tile data that will have to be retreved from server.
//          * Modefied tile data.
//          * Is there any thing placed in tile? building, item, plants, etc...
//          * Other then original tileNoise value texture tile.
//          * Miniras, ores etc...
// 
// 2. Reqest any chunk data from server.
//    This should be done for eatch new chunk
// 
// 3. Recive chunk data
//    Every recived chunk data from server to client needs to update chunk enteties.
// 
// 4. Render chunks and there tiles
//    Should render all tiles in all visible chunks
// 


[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class ChunkManagmentSystem : ComponentSystem
{
    private Entity m_Player;
    private float3 m_Trans;

    private int2 player_coordinats;
    private int2 playerInChunkCoordinats;

    private Material material;

    private int2 chunkGridCoordinats1;
    private int2 chunkGridCoordinats2;

    // should be moved to a level setting singelton.
    // private const int tilesSize = 1; // one tile 
    private const int chunkSize = 16; // amount of tiles in chunk
                                      // vector size ?  8*16 ?



    EntityArchetype reqestLoadChunkEntityArchetype;


    NativeList<Entity> _existingChunks;


    protected override void OnCreate()
    {
        RequireSingletonForUpdate<NetworkIdComponent>();
    }

    protected override void OnUpdate()
    {
        int2[,] _chunks = new int2[3, 3]; // chunks should be loaded

        int2 int2Length = new int2(chunkSize * _chunks.GetLength(0), chunkSize * _chunks.GetLength(1));
        NativeList<Entity> _existingChunks = new NativeList<Entity>(Allocator.Temp);

        // EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype chunkEntityArchetype = EntityManager.CreateArchetype(
           typeof(ChunkComponent),
        //    typeof(ChunkLoadedComponent),
           typeof(Translation),
           typeof(RenderMesh),
           typeof(RenderBounds),
           typeof(LocalToWorld)
       );

        // Get this clients player id
        int localPlayerId = GetSingleton<NetworkIdComponent>().Value;

        int2 chunkGridCoordinats1 = new int2(0, 0);
        int2 chunkGridCoordinats2 = new int2(0, 0);

        int2 playerInChunkCoordinats = new int2(0, 0);

        // Retreves clients player data
        Entities
        .WithAll<PlayerTagComponent>()
        .ForEach((Entity ent, ref PlayerTagComponent player, ref GhostOwnerComponent ghostOwner) =>
        {
            // Is it this clients playerId player id?
            if (ghostOwner.NetworkId == localPlayerId)
            {
                chunkGridCoordinats1 = player.renderingBoundsGridCoordinats1;
                chunkGridCoordinats2 = player.renderingBoundsGridCoordinats2;

                playerInChunkCoordinats = new int2(
                    (player.coordinat.x / chunkSize) * chunkSize,
                    (player.coordinat.y / chunkSize) * chunkSize
                );
            }
        });

        if (material == null)
            Entities.WithAll<MaterialComponent>().ForEach((Entity ent) =>
            {
                material = EntityManager.GetComponentData<MaterialComponent>(ent).material;
            });


        // TODO: fix this plz, needs better offset.
        // TODO: chunkGridCoordinats calculationss dos not work correct!
        // i need to rethink the grid calculation. Chunk grid and tile grid

        chunkGridCoordinats1 = playerInChunkCoordinats - (int2Length / 2);
        chunkGridCoordinats2 = chunkGridCoordinats1 + int2Length;

        // Find missing chunks to create
        NativeList<int2> _missingChunks = new NativeList<int2>(Allocator.Temp);

        // what to create.
        for (int x = 0; x < _chunks.GetLength(0); x++)
        {
            for (int y = 0; y < _chunks.GetLength(1); y++)
            {
                _chunks[x, y] = new int2(
                    (playerInChunkCoordinats.x + (chunkSize * x)) - (int2Length.x / 2),
                    (playerInChunkCoordinats.y + (chunkSize * y)) - (int2Length.x / 2)
                );

                _missingChunks.Add(_chunks[x, y]);
            }
        }

        _existingChunks = new NativeList<Entity>(Allocator.Temp);
        Entities
        .WithAny<ChunkCreateComponent, ChunkLoadComponent, ChunkLoadedComponent, ChunkUnloadedComponent>() // trying to build chunk, load chunk or is loaded
        // .WithAny<ChunkLoadComponent>() // trying to load chunk
        // .WithAny<ChunkLoadedComponent>()
        .ForEach((Entity ent) =>
        {
            // OLD !!!  // _existingChunks.Add(ent); // adds found chunk to nativelist 

            ChunkComponent chunkComponent = EntityManager.GetComponentData<ChunkComponent>(ent);

            for (int i = 0; i < _missingChunks.Length; i++)
            {
                int2 find = _missingChunks[i];
                if (chunkComponent.anchorCoordinats.x == find.x && chunkComponent.anchorCoordinats.y == find.y)
                {
                    _missingChunks.RemoveAt(i);
                }
            }
        });


        // stop if none is missing. this clould be checked before search
        if (_missingChunks.IsEmpty) return;

        // 1. reqest missing to load!
        // 
        foreach (int2 coordinats in _missingChunks)
        {
            // TODO: Sould not create if chunk is unloaded! Else i would need to destory the lasts one...
            // I dont want to loop thorw all unloaded chunks if its missing...
            // This was way i made an reqes to load a chunk before creating the entity
            // 
            Entity entity = EntityManager.CreateEntity(chunkEntityArchetype);

            EntityManager.SetComponentData(entity, new ChunkComponent
            {
                anchorCoordinats = coordinats,
                coordinats = ((coordinats - (chunkSize / 2)) / chunkSize),
                offset = (chunkSize / 2),
                size = chunkSize
            });

            EntityManager.SetComponentData(entity, new Translation
            {
                Value = new float3(((coordinats - (chunkSize / 2)) / chunkSize).x, ((coordinats - (chunkSize / 2)) / chunkSize).y, 0)
            });

            EntityManager.AddComponent<ChunkLoadComponent>(entity); // to reqest load of chunk!
        }

        /*
         * TODO: Reqest chunk data from server!
         */
        _existingChunks.Clear();
        _missingChunks.Clear();
    }

    private bool shouldLoad(int2 coordinats, int2 chunkGridCoordinats1, int2 chunkGridCoordinats2)
    {
        if (coordinats.x > chunkGridCoordinats1.x - 1 &&
            coordinats.x < chunkGridCoordinats2.x &&
            coordinats.y > chunkGridCoordinats1.y - 1 &&
            coordinats.y < chunkGridCoordinats2.y)
        {
            return true;
        }
        return false;
    }

    /*
    * DrawGizmos stuff
    */
    public void OnDrawGizmos()
    {
        Entities.WithAll<ChunkComponent>().ForEach((Entity ent, ref ChunkComponent chunk, ref Translation trans) =>
        {
            if (EntityManager.HasComponent<ChunkLoadedComponent>(ent))
                GizmosDrawSquare(chunk.anchorCoordinats, chunk.anchorCoordinats + chunkSize, Color.green);

            if (EntityManager.HasComponent<ChunkUnloadedComponent>(ent))
                GizmosDrawSquare(chunk.anchorCoordinats, chunk.anchorCoordinats + chunkSize, Color.blue);
        });

        GizmosDrawSquare(chunkGridCoordinats1, chunkGridCoordinats2, Color.white);
    }

    private void GizmosDrawSquare(float3 from, float3 to, Color color)
    {
        Gizmos.color = color;

        // bottom
        Gizmos.DrawLine(
            new float3(from.x, from.y, 0),
            new float3(to.x, from.y, 0)
        );
        // top
        Gizmos.DrawLine(
             new float3(from.x, to.y, 0),
             new float3(to.x, to.y, 0)
         );

        // right
        Gizmos.DrawLine(
            new float3(from.x, from.y, 0),
            new float3(from.x, to.y, 0)
        );
        // left
        Gizmos.DrawLine(
            new float3(to.x, from.y, 0),
            new float3(to.x, to.y, 0)
        );

        Gizmos.DrawCube(from, new float3(.5f, .5f, 0));
    }

    private void GizmosDrawSquare(int2 from, int2 to, Color color)
    {
        GizmosDrawSquare(new float3(from.x, from.y, 0), new float3(to.x, to.y, 0), color);
    }
}


