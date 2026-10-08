using Unity.Entities;

public struct GameState : IComponentData
{
    public bool IsOver;
    public bool RestartRequest;
}