using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct PlayerMoveSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if(SystemAPI.GetSingleton<GameState>().IsOver == true) return;

        foreach(var (transform, speed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Speed>>().WithAll<PlayerTag>())
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var direction = new float2(horizontal, vertical);

            if(horizontal != 0 || vertical != 0)
            {
                math.normalize(direction);
            }

            var xPos = math.clamp(transform.ValueRW.Position.x + direction.x * speed.ValueRO.Value * SystemAPI.Time.DeltaTime, -10, 10); 
            var zPos = math.clamp(transform.ValueRW.Position.z + direction.y * speed.ValueRO.Value * SystemAPI.Time.DeltaTime, -10, 10); 
            transform.ValueRW.Position = new float3(xPos, 0, zPos);
        }
    }
}