using Component;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class RotationSystem : SystemBase
{
    EntityQuery query_boidtarget;

    protected override void OnCreate()
    {
        query_boidtarget = GetEntityQuery(ComponentType.ReadWrite<LocalToWorld>(),
            ComponentType.ReadOnly<HeightComponent>(), ComponentType.ReadOnly<SpeedComponent>(),
            ComponentType.ReadOnly<InitPosComponent>());
    }

    protected override void OnUpdate()
    {
        new QueryJob { ElapsedTime = SystemAPI.Time.ElapsedTime }.ScheduleParallel(query_boidtarget);
    }
}

[BurstCompile]
partial struct QueryJob : IJobEntity
{
    public double ElapsedTime;

    public void Execute(ref LocalToWorld localToWorldTransform, in HeightComponent heightComponent,
        in SpeedComponent speedComponent, in InitPosComponent initPosComponent)
    {
        var elapsedTime = ElapsedTime;
        var heightComponentMaxHeight =
            (float)math.sin((elapsedTime + heightComponent.InitiateHeight) * speedComponent.Speed) *
            heightComponent.MaxHeight;
        var valuePosition = initPosComponent.InitPos;
        valuePosition.y = heightComponentMaxHeight;
        localToWorldTransform.Value = float4x4.Translate(valuePosition);
    }
}