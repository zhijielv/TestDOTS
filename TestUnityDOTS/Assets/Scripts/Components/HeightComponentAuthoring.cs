using Unity.Entities;
using UnityEngine;

namespace Component
{
    public struct HeightComponent : IComponentData
    {
        public float InitiateHeight;
        public float MaxHeight;
    }
    
    public class HeightComponentAuthoring : MonoBehaviour
    {
        public float maxHeight = 1f;
    }

    public class HeightComponentBaker : Baker<HeightComponentAuthoring>
    {
        public override void Bake(HeightComponentAuthoring authoring)
        {
            var heightComponent = new HeightComponent()
            {
                MaxHeight = authoring.maxHeight,
            };
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, heightComponent);
        }
    }
}