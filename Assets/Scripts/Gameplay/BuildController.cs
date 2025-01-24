using System.Collections.Generic;
using Bullastrum.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using ColorUtility = Bullastrum.Utility.ColorUtility;

namespace Bullastrum.Gameplay
{
    public class BuildController : Singleton<BuildController>
    {
        [FormerlySerializedAs("PlanetLayerMask")]
        [Header("Raycast")]
        public LayerMask _planetLayerMask;
        [FormerlySerializedAs("RaycastMaxDistance")]
        public float _raycastMaxDistance;

        [FormerlySerializedAs("Structure")]
        [Header("Prefabs")]
        public GameObject _structure;

        [FormerlySerializedAs("BuildingEnabled")]
        [Header("Debug")]
        public bool _buildingEnabled = true;
        [FormerlySerializedAs("RaycastHitPointRadius")]
        public float _raycastHitPointRadius = 1.0f;
        [FormerlySerializedAs("RaycastHitPointColor")]
        public ColorUtility.Color _raycastHitPointColor;

        private Ray _ray;
        private RaycastHit _raycastHit;
        private Vector3 _raycastHitPoint;
        private GameObject _hoverStructure;
        private List<GameObject> _structures = new List<GameObject>();

        private void Start()
        {
            _hoverStructure = Instantiate(_structure);
        }

        private void Update()
        {
            if (_buildingEnabled)
            {
                _ray = CameraController.Instance.Camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(_ray, out _raycastHit, _raycastMaxDistance, _planetLayerMask))
                {
                    _raycastHitPoint = _raycastHit.point;
                    Vector3 relativePosition = _raycastHitPoint - _raycastHit.transform.position;
                    Quaternion rotation = Quaternion.LookRotation(relativePosition);

                    if (_hoverStructure != null)
                    {
                        _hoverStructure.transform.position = _raycastHitPoint;
                        _hoverStructure.transform.rotation = rotation;

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            _structures.Add(_hoverStructure);
                            _hoverStructure = null;
                        }
                    }
                    else
                    {
                        _hoverStructure = Instantiate(_structure, _raycastHitPoint, rotation, transform);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_raycastHitPoint != Vector3.zero)
            {
                Gizmos.color = ColorUtility.GetColor(_raycastHitPointColor);
                Gizmos.DrawSphere(_raycastHitPoint, _raycastHitPointRadius);
            }
        }
    }
}