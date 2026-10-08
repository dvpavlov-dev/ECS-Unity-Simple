using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct CoinPickupSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        var scoreEntity = state.EntityManager.CreateEntity();
        state.EntityManager.AddComponent<Score>(scoreEntity);
        state.EntityManager.SetName(scoreEntity, "Score");
    }

    public void OnUpdate(ref SystemState state)
    {
        float3 playerPos = float3.zero;
    
        foreach(var playerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag>())
        {
            playerPos = playerTransform.ValueRO.Position;
            break;
        }

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach(var (coinPos, entityCoin) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<CoinTag>().WithEntityAccess())
        {
            if(math.distance(coinPos.ValueRO.Position, playerPos) < 1f)
            {
                SystemAPI.GetSingletonRW<Score>().ValueRW.Value++;
                ecb.DestroyEntity(entityCoin);
                break;
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
