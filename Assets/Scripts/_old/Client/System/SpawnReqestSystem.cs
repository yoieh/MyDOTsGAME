using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

// When client has a connection with network id, go in game and tell server to also go in game
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class SpawnReqestSystem : ComponentSystem
{
    protected override void OnCreate()
    {
    }

    protected override void OnUpdate()
    {
        // // Foreach NetworkIdComponent on client
        // Entities.WithNone<NetworkStreamInGame>().ForEach((Entity ent, ref NetworkIdComponent id) =>
        // {
        //     Debug.Log("ID; " + id.Value);
        //     // Creates NetworkStreamInGame entity with NetworkIdComponent
        //     PostUpdateCommands.AddComponent<NetworkStreamInGame>(ent);
        //     // Creates reqest entity to send to server.
        //     var req = PostUpdateCommands.CreateEntity();
        //     // Adds GoInGameRequest created reqest entity
        //     PostUpdateCommands.AddComponent<SpawnReqest>(req);
        //     // Sends reqest to server
        //     PostUpdateCommands.AddComponent(req, new SendRpcCommandRequestComponent { TargetConnection = ent });
        // });
    }


}
