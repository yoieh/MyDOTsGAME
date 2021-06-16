using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(ChunkBuildingSystem))]
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class ChunkRenderSystem : ComponentSystem
{
    public float width = 1;
    public float height = 1;
    private Material material;
    protected override void OnCreate()
    {
    }

    override protected void OnUpdate()
    {
        if (material == null)
        {
            Entities.WithAll<MaterialComponent>().ForEach((Entity ent) =>
            {
                material = EntityManager.GetComponentData<MaterialComponent>(ent).material;
            });
        }


        Entities.WithAll<ChunkLoadedComponent, ChunkShouldRender>().ForEach((Entity ent, ref ChunkLoadedComponent chunkStateComponent) =>
        {
            // Generates mesh
            Mesh mesh = MyMesh.QuadGrid(16, 16);
            MyMesh.UpdateQuadGridTexture(mesh, 16, 16, material.mainTexture);
            RenderMesh renderMesh = EntityManager.GetSharedComponentData<RenderMesh>(ent);

            // update mesh
            EntityManager.SetSharedComponentData<RenderMesh>(ent, new RenderMesh
            {
                mesh = mesh,//renderMesh.mesh,
                material = material,
            });

            EntityManager.SetComponentData<RenderBounds>(ent, new RenderBounds
            {
                Value = mesh.bounds.ToAABB()
            });

            EntityManager.RemoveComponent<ChunkShouldRender>(ent);
        });



    }
}


