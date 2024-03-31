using UnityEngine;

namespace Behaviour
{
    public class TestStart : MonoBehaviour
    {
        public MyAuthoring Obj;

        [SerializeField] public int count = 2;
        [SerializeField] public float noiseRange = 20f;
        [SerializeField] public float noiseValue = 2f;
        [SerializeField] public float speed = 4f;
        [SerializeField] public float maxHeight = 1f;
        [SerializeField] public Mesh unitMesh;
        [SerializeField] public Material unitMaterial;
        private MyBaker baker;
        public void StartCube()
        {
            baker = new MyBaker()
            {
                count = count,
                noiseRange = noiseRange,
                noiseValue = noiseValue,
                speed = speed,
                maxHeight = maxHeight,
                unitMesh = unitMesh,
                unitMaterial = unitMaterial,
            };
            baker.Bake(Obj);
        }

        public void ReleaseCube()
        {
            baker?.ReleaseCubeList();
        }
    }
}