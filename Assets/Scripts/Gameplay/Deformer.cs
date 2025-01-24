using Bullastrum.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using ColorUtility = Bullastrum.Utility.ColorUtility;

namespace Bullastrum.Gameplay
{
    public class Deformer : MonoBehaviour
    {
        [FormerlySerializedAs("MinScale")]
        [Header("Deform")]
        [Range(0, 1000)]
        public float _minScale = 10;

        [FormerlySerializedAs("MaxScale")]
        [Range(0, 1000)]
        public float _maxScale = 100;

        [FormerlySerializedAs("MinStrength")]
        [Range(0, 1.0f)]
        public float _minStrength = 0.01f;

        [FormerlySerializedAs("MaxStrength")]
        [Range(0, 1.0f)]
        public float _maxStrength = 1.0f;

        [FormerlySerializedAs("Seed")]
        [Header("Noise")]
        public float _seed;
        [FormerlySerializedAs("SeedMultiplier")]
        public float _seedMultiplier = 1;

        [FormerlySerializedAs("RecalculateNormals")]
        [Header("Mesh")]
        public bool _recalculateNormals = true;
        [FormerlySerializedAs("RecalculateTangents")]
        public bool _recalculateTangents = true;

        [FormerlySerializedAs("StartDeformed")]
        [Header("Debug")]
        public bool _startDeformed = false;
        [FormerlySerializedAs("RandomDeformPointRadius")]
        public float _randomDeformPointRadius = 1.0f;
        [FormerlySerializedAs("RandomDeformPointColor")]
        public ColorUtility.Color _randomDeformPointColor;

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

            if (_startDeformed)
            {
                Deform();
            }
        }

        public void Deform()
        {
            float scale = Random.Range(_minScale, _maxScale);
            float strength = Random.Range(_minStrength, _maxStrength);

            transform.localScale = new Vector3(scale, scale, scale);

            var vertices = new Vector3[_baseVertices.Length];

            _seed = Time.time;

            var timeX = _seed * _seedMultiplier;
            var timeY = _seed * _seedMultiplier;
            var timeZ = _seed * _seedMultiplier;

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
            float strength = Random.Range(_minStrength, _maxStrength);

            int randomAmount = Random.Range(0, _baseVertices.Length);

            _currentVertices = _mesh.vertices;

            var randomVertices = new Vector3[randomAmount];

            var timeX = Time.time * _seedMultiplier;
            var timeY = Time.time * _seedMultiplier;
            var timeZ = Time.time * _seedMultiplier;

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
            if (_recalculateNormals)
            {
                _mesh.RecalculateNormals();
            }

            if (_recalculateTangents)
            {
                _mesh.RecalculateTangents();
            }

            _mesh.RecalculateBounds();

            _meshCollider.sharedMesh = _mesh;
        }
    }
}