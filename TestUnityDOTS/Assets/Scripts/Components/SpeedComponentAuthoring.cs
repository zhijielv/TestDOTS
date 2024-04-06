using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Component
{
    public struct SpeedComponent : IComponentData
    {
        public float Speed;
    }
    
    public class SpeedComponentAuthoring : MonoBehaviour
    {
        public float speed = 4f;
    }

    public class SpeedComponentBaker : Baker<SpeedComponentAuthoring>
    {
        public override void Bake(SpeedComponentAuthoring authoring)
        {
            var speedComponent = new SpeedComponent()
            {
                Speed = authoring.speed,
            };
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, speedComponent);
        }
    }
}