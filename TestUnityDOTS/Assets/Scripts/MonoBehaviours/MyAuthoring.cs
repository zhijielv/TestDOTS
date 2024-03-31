using System;
using System.Collections.Generic;
using Component;
using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

namespace Behaviour
{
    public class MyAuthoring : MonoBehaviour
    {
        // [SerializeField] public int count = 100;
        public Mesh unitMesh;
        public Material unitMaterial;
    }

    public class MyBaker : Baker<MyAuthoring>
    {
        public int count = 2;
        public float noiseRange = 20f;
        public float noiseValue = 2f;
        public float speed = 4f;
        public float maxHeight = 1f;
        public Mesh unitMesh;
        public Material unitMaterial;

        private EntitiesGraphicsSystem m_RendererSystem;
        private EntityManager _entityManager;

        private MaterialMeshInfo materialMeshInfo;
        private RenderMeshDescription renderMeshDescription;
        private RenderMeshArray renderMeshArray;

        private Entity _targetEntity;
        private List<Entity> cubeEntityLists;

        public override void Bake(MyAuthoring authoring)
        {
            // var entity = GetEntity(TransformUsageFlags.Dynamic);
            // AddComponent(entity, new SpeedComponent());
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            m_RendererSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<EntitiesGraphicsSystem>();
            var archetype = _entityManager.CreateArchetype(
                typeof(LocalToWorld),
                typeof(HeightComponent),
                typeof(SpeedComponent),
                typeof(MyOwnColor)
            );
            materialMeshInfo = new MaterialMeshInfo()
            {
                MeshID = m_RendererSystem.RegisterMesh(unitMesh),
                MaterialID = m_RendererSystem.RegisterMaterial(unitMaterial),
            };

            renderMeshDescription = new RenderMeshDescription()
            {
                FilterSettings = new RenderFilterSettings()
                {
                    RenderingLayerMask = 1,
                    ShadowCastingMode = ShadowCastingMode.On,
                    ReceiveShadows = true,
                }
            };

            renderMeshArray = new RenderMeshArray(new[] { unitMaterial }, new[] { unitMesh });

            _targetEntity = _entityManager.CreateEntity(archetype);

            ReleaseCubeList();
            cubeEntityLists = new List<Entity>();
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    var pos = new float3(i * 1, 0, j * 1);
                    CreateEntity(pos);
                }
            }

            _entityManager.DestroyEntity(_targetEntity);
        }

        private void CreateEntity(float3 pos)
        {
            float2 float2 = new float2(pos.x / noiseRange, pos.z / noiseRange);
            float f = noise.snoise(float2) / noiseValue;
            pos.y = f;

            var entity = _entityManager.Instantiate(_targetEntity);
            cubeEntityLists.Add(entity);
            var heightComponent = new HeightComponent()
            {
                InitiateHeight = f,
                MaxHeight = maxHeight,
            };
            _entityManager.SetComponentData(entity, heightComponent);

            var speedComponent = new SpeedComponent()
            {
                Speed = speed
            };
            _entityManager.SetComponentData(entity, speedComponent);

            var myOwnColor = new MyOwnColor()
            {
                Value = new float4(),
                Value2 = 1,
            };
            _entityManager.AddComponentData(entity, myOwnColor);
            LocalToWorld toWorldTransform = new LocalToWorld
            {
                Value = float4x4.Translate(pos)
            };
            _entityManager.SetComponentData(entity, toWorldTransform);
            RenderMeshUtility.AddComponents(entity, _entityManager, renderMeshDescription, renderMeshArray,
                MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
        }

        public void ReleaseCubeList()
        {
            if (null == cubeEntityLists || cubeEntityLists.Count == 0)
                return;
            foreach (var cubeEntityList in cubeEntityLists)
            {
                _entityManager.DestroyEntity(cubeEntityList);
            }
        }
    }
}