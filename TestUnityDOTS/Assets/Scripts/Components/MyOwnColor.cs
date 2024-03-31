using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Component
{
    // [MaterialProperty("_Color", MaterialPropertyFormat.Float4)]
    [MaterialProperty("_Color")]
    public struct MyOwnColor : IComponentData
    {
        public float4 Value;
        public float Value2;
    }
}