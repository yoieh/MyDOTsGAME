using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Rendering;
using Unity.Transforms;

[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class ChunkLoadingSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<NetworkIdComponent>();

        // Find the ECB system once and store it for later usage
        m_EndSimulationEcbSystem = World
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        Entity playerEntity = Entity.Null;
        int2 chunkGridCoordinats1 = new int2(0, 0);
        int2 chunkGridCoordinats2 = new int2(0, 0);

        // Get this clients player id
        int localPlayerId = GetSingleton<NetworkIdComponent>().Value;

        // Retreves clients player data
        Entities
        .WithAll<PlayerTagComponent>()
        .ForEach((Entity ent, ref PlayerTagComponent player, in GhostOwnerComponent ghostOwner) =>
        {
            // Is it this clients playerId player id?
            if (ghostOwner.NetworkId == localPlayerId)
            {
                chunkGridCoordinats1 = player.renderingBoundsGridCoordinats1;
                chunkGridCoordinats2 = player.renderingBoundsGridCoordinats2;
                playerEntity = ent;
            }
        })
        .Run();



        EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter(); ;
        NativeList<Entity> _loadChunks = new NativeList<Entity>(Allocator.Temp);

        if (playerEntity != Entity.Null)
        {



            // Get all reqest to load chunks
            Entities
            .WithAll<ChunkLoadComponent>()
            .WithNone<ChunkCreateComponent>()
            .ForEach((Entity ent, int entityInQueryIndex, in ChunkComponent chunk) =>
            {
                if (HasComponent<ChunkUnloadedComponent>(ent))
                {
                    ecb.AddComponent<ChunkLoadedComponent>(entityInQueryIndex, ent);
                    ecb.RemoveComponent<ChunkUnloadedComponent>(entityInQueryIndex, ent);
                    ecb.RemoveComponent<ChunkLoadComponent>(entityInQueryIndex, ent);
                }
                else
                {
                    // _loadChunks.Add(ent);

                    ecb.AddComponent<ChunkCreateComponent>(entityInQueryIndex, ent);
                    ecb.AddComponent<ChunkShouldRender>(entityInQueryIndex, ent);
                    ecb.RemoveComponent<ChunkLoadComponent>(entityInQueryIndex, ent);
                }
            }).ScheduleParallel();


            // Trying to load unloaded chunk if it exists
            // Entities
            // .WithAll<ChunkUnloadedComponent>()
            // // .WithNone<ChunkLoadComponent>()
            // .ForEach((Entity ent, int entityInQueryIndex, in ChunkComponent chunk) =>
            // {
            //     for (int i = 0; i < _loadChunks.Length; i++)
            //     {
            //         ChunkComponent loadChunk = GetComponent<ChunkComponent>(_loadChunks[i]);
            //         if (chunk.coordinats.x == loadChunk.coordinats.x &&
            //             chunk.coordinats.y == loadChunk.coordinats.y)
            //         {
            //             ecb.AddComponent<ChunkLoadComponent>(entityInQueryIndex, ent);
            //             // Dosent sseem to ever run???
            //             ecb.DestroyEntity(entityInQueryIndex, ent);
            //             ecb.DestroyEntity(entityInQueryIndex, _loadChunks[i]);

            //             _loadChunks.RemoveAt(i);
            //             break;
            //         }
            //     }
            // }).Run();


            // if it doses not i need to create a new one!
            // for (int i = 0; i < _loadChunks.Length; i++)
            // {
            //     Entity ent = _loadChunks[i];
            //     if (!HasComponent<ChunkUnloadedComponent>(ent) && !HasComponent<ChunkLoadedComponent>(ent))
            //     {
            //         ecb.AddComponent<ChunkCreateComponent>(ent);
            //         ecb.RemoveComponent<ChunkLoadComponent>(ent);
            //     }
            // }


        }

        // TODO: change to just unload loaded chunks that should be unloaded!
        // Just loads and unloads chunks from memory
        Entities.WithAll<ChunkLoadedComponent>().ForEach((Entity ent, int entityInQueryIndex, in ChunkComponent chunk) =>
        {
            if (
                chunk.anchorCoordinats.x > chunkGridCoordinats1.x - 1 &&
                chunk.anchorCoordinats.x < chunkGridCoordinats2.x &&
                chunk.anchorCoordinats.y > chunkGridCoordinats1.y - 1 &&
                chunk.anchorCoordinats.y < chunkGridCoordinats2.y)
            {
            }
            else
            {
                ecb.RemoveComponent<ChunkLoadedComponent>(entityInQueryIndex, ent);
                ecb.AddComponent<ChunkUnloadComponent>(entityInQueryIndex, ent);
            }
        }).ScheduleParallel();


        // Destroy unchanged thats up for unloading
        Entities.WithAll<ChunkUnloadComponent>().WithNone<ChunkHasChanged>().ForEach((Entity ent, int entityInQueryIndex) =>
        {
            ecb.DestroyEntity(entityInQueryIndex, ent);
        }).ScheduleParallel();

        // Unload changes
        Entities.WithAll<ChunkUnloadComponent, ChunkHasChanged>().ForEach((Entity ent, int entityInQueryIndex, in ChunkComponent chunk) =>
        {
            // TODO changes to server!
            ecb.DestroyEntity(entityInQueryIndex, ent);
        }).ScheduleParallel();
    }
}
