using Unity.NetCode;

public struct PlayerInputCommandData : ICommandData
{
    public uint Tick { get; set; }
    public int horizontal;
    public int vertical;
}