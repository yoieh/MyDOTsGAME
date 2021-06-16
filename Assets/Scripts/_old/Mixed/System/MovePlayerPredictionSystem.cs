using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
public class MovePlayerPredictionSystem : ComponentSystem
{
    private const int gridSize = 1; // one tile 
    private const int chunkSize = 16; // one tile 

    protected override void OnUpdate()
    {
        // Prediction for players movments

        // Gets existing system group for GhostPrediction
        GhostPredictionSystemGroup group = World.GetExistingSystem<GhostPredictionSystemGroup>();

        // gets the groups predicting tick
        uint tick = group.PredictingTick;
        // Just the delta time
        float deltaTime = Time.DeltaTime;

        // lopping all entities with PlayerInputCommandData dynamicBuffers that needs to be predicted. All players.
        Entities.ForEach((DynamicBuffer<PlayerInputCommandData> inputBuffer, ref Translation trans, ref PredictedGhostComponent prediction, ref PlayerTagComponent player) =>
        {
            // Check if movments should be predicted
            if (!GhostPredictionSystemGroup.ShouldPredict(tick, prediction))
                return;

            // Get input data for a PlayerInputCommandData
            inputBuffer.GetDataAtTick(tick, out PlayerInputCommandData input);

            // current inputs to predicted
            if (input.horizontal > 0)
                trans.Value.x += (player.speed * deltaTime);
            if (input.horizontal < 0)
                trans.Value.x -= (player.speed * deltaTime);
            if (input.vertical > 0)
                trans.Value.y += (player.speed * deltaTime);
            if (input.vertical < 0)
                trans.Value.y -= (player.speed * deltaTime);

            player.coordinat = new int2(
                Mathf.RoundToInt(trans.Value.x / gridSize) * gridSize,
                Mathf.RoundToInt(trans.Value.y / gridSize) * gridSize
            );

            player.chunkCoordinat = new int2(
                Mathf.RoundToInt(trans.Value.x / chunkSize) * chunkSize,
                Mathf.RoundToInt(trans.Value.y / chunkSize) * chunkSize
            );

        });
    }
}