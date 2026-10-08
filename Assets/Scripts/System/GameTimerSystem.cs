using Unity.Entities;

public partial struct GameTimerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        var gameTimerEntity = state.EntityManager.CreateEntity();
        state.EntityManager.AddComponent<GameTimer>(gameTimerEntity);
        state.EntityManager.SetComponentData(gameTimerEntity, new GameTimer{ Value = 10 });
        state.EntityManager.SetName(gameTimerEntity, "GameTimer");
    }

    public void OnUpdate(ref SystemState state)
    {
        if(SystemAPI.GetSingleton<GameState>().IsOver == true) return;

        var gameTimer = SystemAPI.GetSingletonRW<GameTimer>();

        if(gameTimer.ValueRO.Value >= 0.1f)
        {
            SystemAPI.GetSingletonRW<GameTimer>().ValueRW.Value -= SystemAPI.Time.DeltaTime;
        }
        else
        {
            SystemAPI.GetSingletonRW<GameTimer>().ValueRW.Value = 0;
            SystemAPI.GetSingletonRW<GameState>().ValueRW.IsOver = true;
        }
    }
}