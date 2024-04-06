using Component;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Components
{
    public struct EntityPrefabComponent : IComponentData
    {
        public Entity Value;
        public int Count;
        public float NoiseRange;
        public float NoiseValue;
    }

    public class GetPrefabAuthoring : MonoBehaviour
    {
        public int count = 2;
        public float noiseRange = 20f;
        public float noiseValue = 2f;
        public GameObject prefab;
    }

    public class GetPrefabReferenceBaker : Baker<GetPrefabAuthoring>
    {
        private GameObject _authoring;

        public override void Bake(GetPrefabAuthoring authoring)
        {
            _authoring = authoring.gameObject;
            var entityPrefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic);
            var entityPrefabComponent = new EntityPrefabComponent()
            {
                Value = entityPrefab,
                Count = authoring.count,
                NoiseRange = authoring.noiseRange,
                NoiseValue = authoring.noiseValue,
            };

            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, entityPrefabComponent);
        }
    }

    public partial class InstantiatePrefabSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<EntityPrefabComponent>();
        }

        protected override void OnUpdate()
        {
            Enabled = false;
            var entityPrefabComponent = SystemAPI.GetSingleton<EntityPrefabComponent>();
            var noiseRange = entityPrefabComponent.NoiseRange;
            var noiseValue = entityPrefabComponent.NoiseValue;
            var count = TestButton.Count;
            TestButton.Stopwatch.Restart();
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    var entity = EntityManager.Instantiate(entityPrefabComponent.Value);
                    var pos = new float3(i * 1, 0, j * 1);
                    var initPosComponent = EntityManager.GetComponentData<InitPosComponent>(entity);
                    initPosComponent.InitPos = pos;
                    EntityManager.SetComponentData(entity, initPosComponent);

                    float2 float2 = new float2(pos.x / noiseRange, pos.z / noiseRange);
                    float f = noise.snoise(float2) / noiseValue;
                    pos.y = f;
                    var heightComponent = EntityManager.GetComponentData<HeightComponent>(entity);
                    heightComponent.InitiateHeight = f;
                    EntityManager.SetComponentData(entity, heightComponent);
                }
            }

            TestButton.Stopwatch.Stop();
        }
    }

    // public partial struct DestroyPrefabSystem : ISystem
    public partial class DestroyPrefabSystem : SystemBase
    {
        // public void OnCreate(ref SystemState state)
        protected override void OnCreate()
        {
            // EnablePrefab = false;
        }

        // public void OnUpdate(ref SystemState state)
        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (component, entity) in SystemAPI.Query<RefRO<SpeedComponent>>().WithEntityAccess())
            {
                ecb.DestroyEntity(entity);
            }

            // ecb.Playback(state.EntityManager);
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}