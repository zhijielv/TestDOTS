using Component;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
/// <summary>
/// "com.unity.entities.graphics": "1.0.10",
/// </summary>
public partial class RotationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref LocalToWorld localToWorldTransform, in HeightComponent heightComponent,
            in SpeedComponent speedComponent) =>
        {
            var elapsedTime = SystemAPI.Time.ElapsedTime;
            var heightComponentMaxHeight =
                (float) math.sin((elapsedTime + heightComponent.InitiateHeight) * speedComponent.Speed) *
                heightComponent.MaxHeight;
            var valuePosition = localToWorldTransform.Position;
            valuePosition.y = heightComponentMaxHeight;
            localToWorldTransform.Value = float4x4.Translate(valuePosition);
        }).WithoutBurst().Run();
    }
}