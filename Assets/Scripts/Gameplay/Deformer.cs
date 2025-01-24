using Bullastrum.Utility;
using UnityEngine;
using ColorUtility = Bullastrum.Utility.ColorUtility;

namespace Bullastrum.Gameplay
{
    public class Deformer : MonoBehaviour
    {
        [Header("Deform")]
        [Range(0, 1000)]
        public float MinScale = 10;

        [Range(0, 1000)]
        public float MaxScale = 100;

        [Range(0, 1.0f)]
        public float MinStrength = 0.01f;

        [Range(0, 1.0f)]
        public float MaxStrength = 1.0f;

        [Header("Noise")]
        public float Seed;
        public float SeedMultiplier = 1;

        [Header("Mesh")]
        public bool RecalculateNormals = true;
        public bool RecalculateTangents = true;

        [Header("Debug")]
        public bool StartDeformed = false;
        public float RandomDeformPointRadius = 1.0f;
        public ColorUtility.Color RandomDeformPointColor;

        private Mesh _mesh;
        private MeshCollider _meshCollider;
        private Vector3[] _baseVertices;
        private Vector3[] _currentVertices;

        private readonly Perlin _noise = new Perlin();

        private void Start()
        {
            _mesh = GetComponent<MeshFilter>().mesh;
            _meshCollider = GetComponent<MeshCollider>();
            _baseVertices = _mesh.vertices;

            if (StartDeformed)
            {
                Deform();
            }
        }

        public void Deform()
        {
            float scale = Random.Range(MinScale, MaxScale);
            float strength = Random.Range(MinStrength, MaxStrength);

            transform.localScale = new Vector3(scale, scale, scale);

            var vertices = new Vector3[_baseVertices.Length];

            Seed = Time.time;

            var timeX = Seed * SeedMultiplier;
            var timeY = Seed * SeedMultiplier;
            var timeZ = Seed * SeedMultiplier;

            for (var i = 0; i < vertices.Length; i++)
            {
                var vertex = _baseVertices[i];

                vertex.x += _noise.Noise(timeX + vertex.x, timeX + vertex.y, timeX + vertex.z) * strength;
                vertex.y += _noise.Noise(timeY + vertex.x, timeY + vertex.y, timeY + vertex.z) * strength;
                vertex.z += _noise.Noise(timeZ + vertex.x, timeZ + vertex.y, timeZ + vertex.z) * strength;

                vertices[i] = vertex;
            }

            _mesh.vertices = vertices;

            RecalculateMesh();
        }

        public void Undeform()
        {
            _mesh.vertices = _baseVertices;

            RecalculateMesh();
        }

        public void Randomize()
        {
            float strength = Random.Range(MinStrength, MaxStrength);

            int randomAmount = Random.Range(0, _baseVertices.Length);

            _currentVertices = _mesh.vertices;

            var randomVertices = new Vector3[randomAmount];

            var timeX = Time.time * SeedMultiplier;
            var timeY = Time.time * SeedMultiplier;
            var timeZ = Time.time * SeedMultiplier;

            for (int i = 0; i < randomVertices.Length; i++)
            {
                var vertex = _currentVertices[i];

                vertex.x += _noise.Noise(timeX + vertex.x, timeX + vertex.y, timeX + vertex.z) * strength;
                vertex.y += _noise.Noise(timeY + vertex.x, timeY + vertex.y, timeY + vertex.z) * strength;
                vertex.z += _noise.Noise(timeZ + vertex.x, timeZ + vertex.y, timeZ + vertex.z) * strength;

                randomVertices[i] = vertex;

                _currentVertices[i] = vertex;
            }

            _mesh.vertices = _currentVertices;

            RecalculateMesh();
        }

        private void RecalculateMesh()
        {
            if (RecalculateNormals)
            {
                _mesh.RecalculateNormals();
            }

            if (RecalculateTangents)
            {
                _mesh.RecalculateTangents();
            }

            _mesh.RecalculateBounds();

            _meshCollider.sharedMesh = _mesh;
        }
    }
}