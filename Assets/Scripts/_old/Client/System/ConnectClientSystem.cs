using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using Unity.Burst;
using UnityEngine;
using System;

// Control system updating in the default world
[UpdateInWorld(UpdateInWorld.TargetWorld.Default)]
public class ConnectClientSystem : ComponentSystem
{
    // Singleton component to trigger connections once from a control system

    protected override void OnCreate()
    {
        RequireSingletonForUpdate<ConnectClientComponent>();
        // Create singleton, require singleton for update so system runs once

    }

    protected override void OnUpdate()
    {

        // ushort port = Convert.ToUInt16(GetSingleton<ConnectClientComponent>().port);

        // foreach (var world in World.All)
        // {
        //     var network = world.GetExistingSystem<NetworkStreamReceiveSystem>();
        //     if (world.GetExistingSystem<ClientSimulationSystemGroup>() != null)
        //     {
        //         // Client worlds automatically connect to localhost
        //         NetworkEndPoint ep = NetworkEndPoint.LoopbackIpv4;
        //         ep.Port = port;
        //         network.Connect(ep);
        //         Debug.Log("Client Connect @ " + ep.Address);
        //     }
        // }

        // // Destroy singleton to prevent system from running again
        // EntityManager.DestroyEntity(GetSingletonEntity<ConnectClientComponent>());
    }
}


public struct ConnectClientComponent : IComponentData
{
    public ushort port;
    // public string username;
    // public string ip;
}