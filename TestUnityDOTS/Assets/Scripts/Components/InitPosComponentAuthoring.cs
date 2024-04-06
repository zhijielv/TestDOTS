using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Component
{
    public struct InitPosComponent : IComponentData
    {
        public float3 InitPos;
    }
    
    public class InitPosComponentAuthoring : MonoBehaviour
    {
    }

    public class InitPosComponentBaker : Baker<InitPosComponentAuthoring>
    {
        public override void Bake(InitPosComponentAuthoring authoring)
        {
            var InitPosComponent = new InitPosComponent();
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, InitPosComponent);
        }
    }
}