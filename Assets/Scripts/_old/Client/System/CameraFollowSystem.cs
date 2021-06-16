using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class CameraFollowSystem : SystemBase
{
    private bool firstFrame = true;
    private Vector3 offset;

    protected override void OnUpdate()
    {
        int localPlayerId = GetSingleton<NetworkIdComponent>().Value;

        Entities.WithoutBurst().ForEach((Entity entity, ref Translation translation, in GhostOwnerComponent ghostOwner) =>
        {
            // Is it this clients playerId player id?
            if (ghostOwner.NetworkId == localPlayerId)
            {
                var mainCamera = Camera.main;
                if (mainCamera == null)
                    return;



                var playerPos = new Vector3(translation.Value.x, translation.Value.y);

                if (firstFrame)
                {
                    offset = mainCamera.transform.position - new Vector3(playerPos.x, playerPos.y);
                    firstFrame = false;
                }

                var smoothing = 5f; // SurvivalShooterBootstrap.Settings.CamSmoothing;
                var dt = Time.DeltaTime;
                var targetCamPos = playerPos + offset;
                mainCamera.transform.position =
                    Vector3.Lerp(mainCamera.transform.position, targetCamPos, smoothing * dt);
            }
        }).Run();
    }
}
