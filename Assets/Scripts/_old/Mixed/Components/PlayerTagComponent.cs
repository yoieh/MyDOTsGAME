using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct PlayerTagComponent : IComponentData
{

    [GhostField]
    public int2 coordinat;
    
    [GhostField]
    public int2 chunkCoordinat;


    // TODO: should move this to singelton setting?
    public int renderingDistance; // chunks should be loaded


    public int2 renderingBoundsGridCoordinats1;
    public int2 renderingBoundsGridCoordinats2;

    public int speed;

}