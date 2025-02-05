﻿using Bullastrum.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bullastrum.Gameplay
{
    public class Deformer : MonoBehaviour
    {
        [Header("Deform")]
        [Range(0, 1000)]
        [SerializeField] private float _minScale = 10;
        
        [Range(0, 1000)]
        [SerializeField] private float _maxScale = 100;
        
        [Range(0, 1.0f)]
        [SerializeField] private float _minStrength = 0.01f;
        
        [Range(0, 1.0f)]
        [SerializeField] private float _maxStrength = 1.0f;
        
        [Header("Noise")]
        [SerializeField] private float _seed;
        [SerializeField] private float _seedMultiplier = 1;
        
        [Header("Mesh")]
        [SerializeField] private bool _recalculateNormals = true;
        [SerializeField] private bool _recalculateTangents = true;
        
        [Header("Material")]
        [SerializeField] private Material _material;

        [Header("Settings")]
        [SerializeField] private bool _changeLightDirection = true;
        [SerializeField] private float _lightChangeSpeed = 0.1f;
        
        [Header("Debug")]
        [SerializeField] private bool _startDeformed = true;

        private Mesh _mesh;
        private MeshCollider _meshCollider;
        private Vector3[] _baseVertices;
        private Vector3[] _currentVertices;
        private float _timer;

        private readonly Perlin _noise = new Perlin();
        
        private static readonly int Seed = Shader.PropertyToID("_Seed");
        private static readonly int LightDirection = Shader.PropertyToID("_LightDir");

        private void Start()
        {
            _mesh = GetComponent<MeshFilter>().mesh;
            _meshCollider = GetComponent<MeshCollider>();
            _baseVertices = _mesh.vertices;

            if (_startDeformed)
            {
                Deform();
                Noise();
            }
        }

        private void Update()
        {
            if (_changeLightDirection)
            {
                _timer += Time.deltaTime;
                float x = Mathf.Cos(_timer * _lightChangeSpeed);
                float y = Mathf.Cos(_timer * _lightChangeSpeed);
                UpdateLightDirection(new Vector3(x, y, -0.5f));
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

        public void Noise()
        {
            float seedValue = Mathf.PingPong(Time.time * 1.0f, 1.0f);
            _material.SetFloat(Seed, seedValue);
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
        
        public void UpdateLightDirection(Vector3 direction)
        {
            _material.SetVector(LightDirection, direction);
        }
    }
}