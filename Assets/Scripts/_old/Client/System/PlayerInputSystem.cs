using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class PlayerInputSystem : ComponentSystem
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<NetworkIdComponent>();
    }

    protected override void OnUpdate()
    {
        // // Client input system

        // // Gets localInput entity
        // Entity localInput = GetSingleton<CommandTargetComponent>().targetEntity;
        // if (localInput == Entity.Null) // set up for local input 
        // {
        //     // Get this clients player id
        //     int localPlayerId = GetSingleton<NetworkIdComponent>().Value;
        //     // Retreve all the cilents GhostOwnerComponent
        //     Entities.WithAll<PlayerTagComponent>().WithNone<PlayerInputCommandData>().ForEach((Entity ent, ref GhostOwnerComponent ghostOwner) =>
        //     {
        //         // Is it this clients playerId player id?
        //         if (ghostOwner.NetworkId == localPlayerId)
        //         {
        //             // player input data
        //             PostUpdateCommands.AddBuffer<PlayerInputCommandData>(ent);
        //             // Set up for sending commands
        //             PostUpdateCommands.SetComponent(GetSingletonEntity<CommandTargetComponent>(), new CommandTargetComponent { targetEntity = ent });
        //         }
        //     });
        //     return;
        // }

        // // Get a empty player input data
        // PlayerInputCommandData input = default(PlayerInputCommandData);

        // // Setting the tick from clients server tick
        // input.Tick = World.GetExistingSystem<ClientSimulationSystemGroup>().ServerTick;

        // // Handels inputs from user
        // if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        // {
        //     // transform.position += new Vector3(-1, 0) * moveSpeed * Time.deltaTime;
        //     input.horizontal -= 1;
        // }
        // if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        // {
        //     // transform.position += new Vector3(+1, 0) * moveSpeed * Time.deltaTime;
        //     input.horizontal += 1;
        // }
        // if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        // {
        //     // transform.position += new Vector3(0, -1) * moveSpeed * Time.deltaTime;
        //     input.vertical -= 1;
        // }
        // if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        // {
        //     // transform.position += new Vector3(0, +1) * moveSpeed * Time.deltaTime;
        //     input.vertical += 1;
        // }

        // // TODO: camera zoome system when i have a camera follow system that works
        // // if (Input.GetKeyDown(KeyCode.KeypadPlus) && zoome > 10)
        // // {
        // //     zoome -= 10f;
        // // }
        // // if (Input.GetKeyDown(KeyCode.KeypadMinus) && zoome < 10)
        // // {
        // //     zoome += 10f;
        // // }

        // // Addes input to localInput
        // DynamicBuffer<PlayerInputCommandData> inputBuffer = EntityManager.GetBuffer<PlayerInputCommandData>(localInput);
        // inputBuffer.AddCommandData(input);
    }
}