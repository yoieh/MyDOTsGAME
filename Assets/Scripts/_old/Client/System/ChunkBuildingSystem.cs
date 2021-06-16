using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class ChunkBuildingSystem : SystemBase
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
        EntityCommandBuffer ecb = m_EndSimulationEcbSystem.CreateCommandBuffer();

        // Get all reqest to load chunks
        Entities
        .WithAll<ChunkCreateComponent>()
        .ForEach((Entity ent, int entityInQueryIndex, in ChunkComponent chunk) =>
        {
            // ecb.SetComponent(ent, chunk);
            
            // TODO: need to translate thiss from chunkCoordinats
            ecb.SetComponent(ent, new Translation
            {
                Value = new float3(chunk.anchorCoordinats.x, chunk.anchorCoordinats.y, 0)
            });

            // TODO: move to rendering system, mesh data could be saved on chunk bot not on RenderMesh
            // Generates mesh
            // Mesh mesh = MeshExtension.QuadGrid(16, 16);
            // ecb.SetSharedComponent<RenderMesh>(entity, new RenderMesh
            // {
            //     mesh = mesh,
            //     // material = material
            // });


            //             ecb.SetComponent<RenderBounds>(entity, new RenderBounds
            //             {
            //                 // Value = mesh.bounds.ToAABB()
            //             });

            // #if UNITY_EDITOR
            //             // ecb.SetName(entity, "chunk: " + ((coordinats - (chunkSize / 2)) / chunkSize));
            // #endif

            ecb.RemoveComponent<ChunkCreateComponent>(ent);
            ecb.AddComponent<ChunkLoadedComponent>(ent);
        }).Run();
    }
}
