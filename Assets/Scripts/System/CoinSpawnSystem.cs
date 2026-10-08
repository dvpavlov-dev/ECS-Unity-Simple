using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct CoinSpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }

    public void OnUpdate(ref SystemState state)
    {
        if(SystemAPI.GetSingleton<GameState>().IsOver == true) return;

        var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                            .CreateCommandBuffer(state.WorldUnmanaged);

        foreach(var data in SystemAPI.Query<RefRW<SpawnerData>>())
        {
            data.ValueRW.Timer += SystemAPI.Time.DeltaTime;

            if(data.ValueRW.Timer >= data.ValueRO.IntervalSpawn)
            {
                data.ValueRW.Timer = 0;

                var entityCoin = ecb.Instantiate(data.ValueRO.CoinPrefab);

                var pos = new float3(data.ValueRW.RandomForPos.NextFloat(-10, 10), 0, data.ValueRW.RandomForPos.NextFloat(-10, 10));
                ecb.SetComponent<LocalTransform>(entityCoin, new LocalTransform
                {
                    Position = pos,
                    Rotation = Quaternion.identity,
                    Scale = 1
                });
            } 
        }
    }
}