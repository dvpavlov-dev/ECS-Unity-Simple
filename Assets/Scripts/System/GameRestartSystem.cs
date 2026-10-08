using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct GameRestartSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        var entity = state.EntityManager.CreateEntity();
        state.EntityManager.AddComponent<GameState>(entity);
        state.EntityManager.SetName(entity, "GameState");
    }

    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingletonRW<GameState>();
        if(gameState.ValueRO.RestartRequest == true)
        {
            gameState.ValueRW.IsOver = false;
            gameState.ValueRW.RestartRequest = false;

            SystemAPI.GetSingletonRW<Score>().ValueRW.Value = 0;
            SystemAPI.GetSingletonRW<GameTimer>().ValueRW.Value = 10;

            foreach(var data in SystemAPI.Query<RefRW<SpawnerData>>())
            {
                data.ValueRW.Timer = 0;
            }

            foreach(var playerTransform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<PlayerTag>())
            {
                playerTransform.ValueRW.Position = float3.zero;
            }

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (coin, entity) in SystemAPI.Query<CoinTag>().WithEntityAccess())
            {
                ecb.DestroyEntity(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}