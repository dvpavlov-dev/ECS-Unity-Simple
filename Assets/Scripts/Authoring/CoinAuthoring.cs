using Unity.Entities;
using UnityEngine;

public class CoinAuthoring : MonoBehaviour
{
    private class Baker : Baker<CoinAuthoring>
    {
        public override void Bake(CoinAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new CoinTag());
        }
    }
}
