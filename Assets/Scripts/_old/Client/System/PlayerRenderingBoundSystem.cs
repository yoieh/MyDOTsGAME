using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.NetCode;


[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class PlayerRenderingBoundsSystem : SystemBase
{
    private const int chunkSize = 16; // amount of tiles in chunk

    protected override void OnUpdate()
    {
        int localPlayerId = GetSingleton<NetworkIdComponent>().Value;

        // Retreves clients player data
        Entities
        .WithAll<PlayerTagComponent>()
            .ForEach((Entity ent, ref PlayerTagComponent player, in GhostOwnerComponent ghostOwner) =>
            {
                // Is it this clients playerId player id?
                if (ghostOwner.NetworkId == localPlayerId)
                {
                    if (player.renderingDistance == 0) player.renderingDistance = 3;

                    int2 playerInChunkCoordinats = new int2(
                        (player.coordinat.x / chunkSize) * chunkSize,
                        (player.coordinat.y / chunkSize) * chunkSize
                    );
                    int2 int2RenderDictance = new int2(chunkSize * player.renderingDistance, chunkSize * player.renderingDistance);

                    player.renderingBoundsGridCoordinats1 = playerInChunkCoordinats - (int2RenderDictance / 2);
                    player.renderingBoundsGridCoordinats2 = player.renderingBoundsGridCoordinats1 + int2RenderDictance;

                }
            })
            .Schedule();
    }
}
