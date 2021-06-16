using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public class TestingChunkManager : MonoBehaviour
{
    ChunkManagmentSystem system;

    private void Update()
    {
        if (system == null)
        {
            foreach (World world in World.All)
            {
                ChunkManagmentSystem _system = world.GetExistingSystem<ChunkManagmentSystem>();
                if (_system != null)
                {
                    system = _system;
                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (system != null)
        {
            system.OnDrawGizmos();
        }

    }
}
