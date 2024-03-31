using Unity.Entities;
using Unity.Transforms;

namespace Component
{
    public struct HeightComponent : IComponentData
    {
        public float InitiateHeight;
        public float MaxHeight;
    }
}