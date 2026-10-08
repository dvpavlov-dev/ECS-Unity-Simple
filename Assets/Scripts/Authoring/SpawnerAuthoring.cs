using Unity.Entities;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private float _intervalCoinSpawn;

    private class Baker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            var spawnEntity = GetEntity(TransformUsageFlags.None);
            var coinEntity = GetEntity(authoring._coinPrefab, TransformUsageFlags.Dynamic);

            AddComponent(spawnEntity, new SpawnerData{ 
                CoinPrefab = coinEntity, 
                IntervalSpawn = authoring._intervalCoinSpawn,
                Timer = 0,
                RandomForPos = Unity.Mathematics.Random.CreateFromIndex(123)});
        }
    }
}
