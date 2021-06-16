using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class ChunkReqestSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        // Foreach NetworkIdComponent on client
        Entities.WithNone<NetworkStreamInGame>().ForEach((Entity ent, ref NetworkIdComponent id) =>
        {
            // // Creates reqest entity to send to server.
            // var req = PostUpdateCommands.CreateEntity();
            // // // Adds GoInGameRequest created reqest entity
            // PostUpdateCommands.AddComponent<ChunkReqest>(req);
            // // // Sends reqest to server
            // PostUpdateCommands.AddComponent(req, new SendRpcCommandRequestComponent { TargetConnection = ent });
        });
    }
}