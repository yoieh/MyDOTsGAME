using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using Unity.Burst;
using UnityEngine;
using System;

// Control system updating in the default world
[UpdateInWorld(UpdateInWorld.TargetWorld.Default)]
public class StartServerSystem : ComponentSystem
{
    // Singleton component to trigger connections once from a control system

    protected override void OnCreate()
    {
        RequireSingletonForUpdate<StartServerComponent>();
        // Create singleton, require singleton for update so system runs onc
    }

    protected override void OnUpdate()
    {
        // bool listening = false;
        // NetworkEndPoint ep = NetworkEndPoint.AnyIpv4;
        // ep.Port = Convert.ToUInt16(GetSingleton<StartServerComponent>().port);

        // // Destroy singleton to prevent system from running again
        // foreach (var world in World.All)
        // {
        //     var network = world.GetExistingSystem<NetworkStreamReceiveSystem>();
        //     if (world.GetExistingSystem<ServerSimulationSystemGroup>() != null)
        //     {
        //         // Server world automatically listens for connections from any host
        //         listening = network.Listen(ep);
        //     }

        // }
        // if (listening)
        // {
        //     Debug.Log("Server Listen @ " + ep.Address);
        //     EntityManager.DestroyEntity(GetSingletonEntity<StartServerComponent>());
        // }
    }
}


public struct StartServerComponent : IComponentData
{
    public ushort port;
}