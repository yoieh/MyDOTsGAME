using System;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;


[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class OLDPlayerSpawnSystem : ComponentSystem
{
    // private Entity m_Prefab;

    // protected override void OnCreate()
    // {
    //     RequireSingletonForUpdate<GhostPrefabCollectionComponent>();
    // }


    protected override void OnUpdate()
    {

        //     if (m_Prefab == Entity.Null)
        //     {
        //         Entity prefabEntity = GetSingletonEntity<GhostPrefabCollectionComponent>();
        //         DynamicBuffer<GhostPrefabBuffer> prefabs = EntityManager.GetBuffer<GhostPrefabBuffer>(prefabEntity);
        //         for (int i = 0; i < prefabs.Length; ++i)
        //         {
        //             // Gets just PlayerTagComponent Entitys
        //             if (EntityManager.HasComponent<PlayerTagComponent>(prefabs[i].Value))
        //                 m_Prefab = prefabs[i].Value;
        //         }
        //         if (m_Prefab == Entity.Null)
        //             return;
        //     }

        //     // Foreach GoInGameRequest from client
        //     Entities.WithNone<SendRpcCommandRequestComponent>().ForEach((Entity reqEnt, ref SpawnReqest req, ref ReceiveRpcCommandRequestComponent reqSrc) =>
        //     {
        //         Debug.Log(String.Format("Server setting connection {0} to in game", EntityManager.GetComponentData<NetworkIdComponent>(reqSrc.SourceConnection).Value));

        //         // Rpc Reqest recived from a client adds network component
        //         PostUpdateCommands.AddComponent<NetworkStreamInGame>(reqSrc.SourceConnection);

        //         // instantiates player entity from prefab
        //         Entity player = EntityManager.Instantiate(m_Prefab);

        //         // Sets network id on GhostOwnerComponent of player entity
        //         EntityManager.SetComponentData(player, new GhostOwnerComponent
        //         {
        //             // NetworkId obtained from reqSrc
        //             NetworkId = EntityManager.GetComponentData<NetworkIdComponent>(reqSrc.SourceConnection).Value
        //         });

        //         // Adds PlayerInputCommandData data to player
        //         PostUpdateCommands.AddBuffer<PlayerInputCommandData>(player);

        //         // Respond to player entity?
        //         PostUpdateCommands.SetComponent(reqSrc.SourceConnection, new CommandTargetComponent { targetEntity = player });

        //         // Destroy entity reqest from client
        //         PostUpdateCommands.DestroyEntity(reqEnt);

        //     });

    }
}