using Unity.Entities;
using Unity.Mathematics;

public struct SpawnerData : IComponentData
{
    public Entity CoinPrefab;
    public float IntervalSpawn;
    public float Timer;
    public Random RandomForPos;
}
