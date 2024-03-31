/*
using Component;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Behaviour
{
    public class StartBehaviour : MonoBehaviour
    {
        [SerializeField] public int count = 100;
        [SerializeField] public float noiseRange = 20f;
        [SerializeField] public float noiseValue = 2f;
        [SerializeField] public float speed = 4f;
        [SerializeField] public float maxHeight = 1f;
        [SerializeField] private Mesh unitMesh;
        [SerializeField] private Material unitMaterial;

        private EntityManager _entityManager;

        void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var archetype = _entityManager.CreateArchetype(
                typeof(HeightComponent),
                typeof(SpeedComponent),
                typeof(RenderBounds),
                typeof(RenderMesh)
            );
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    var pos = new float3(i * 2, 0, j * 2);
                    CreateEntity(archetype, pos);
                }
            }
        }

        private void CreateEntity(EntityArchetype archetype, float3 pos)
        {
            float2 float2 = new float2(pos.x / noiseRange, pos.z / noiseRange);
            float f = noise.snoise(float2) / noiseValue;
            pos.y = f;
            Entity entity = _entityManager.CreateEntity(archetype);

            CreateGameObject(ref entity, pos);

            _entityManager.AddComponentData(entity, new HeightComponent()
            {
                InitiateHeight = f,
                MaxHeight = maxHeight,
            });
            _entityManager.AddComponentData(entity, new SpeedComponent()
            {
                Speed = speed
            });
        }

        private void CreateGameObject(ref Entity entity, float3 pos)
        {
            var t = new LocalToWorldTransform { Value = UniformScaleTransform.FromPosition(pos) };
            
            _entityManager.AddSharedComponentManaged(entity, new RenderMesh
            {
                mesh = unitMesh,
                material = unitMaterial,
            });
        }
    }
}
*/
