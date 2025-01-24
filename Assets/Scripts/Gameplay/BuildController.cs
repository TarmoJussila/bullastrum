using System.Collections.Generic;
using Bullastrum.Utility;
using UnityEngine;
using ColorUtility = Bullastrum.Utility.ColorUtility;

namespace Bullastrum.Gameplay
{
    public class BuildController : Singleton<BuildController>
    {
        [Header("Raycast")]
        [SerializeField] private LayerMask _planetLayerMask;
        [SerializeField] private float _raycastMaxDistance;
        [SerializeField] private Transform _planetTransform;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _structure;
        
        [Header("Debug")]
        [SerializeField] private bool _buildingEnabled = true;
        [SerializeField] private float _raycastHitPointRadius = 1.0f;
        [SerializeField] private ColorUtility.Color _raycastHitPointColor;

        private Ray _ray;
        private RaycastHit _raycastHit;
        private Vector3 _raycastHitPoint;
        private GameObject _hoverStructure;
        private List<GameObject> _structures = new List<GameObject>();

        private void Start()
        {
            _hoverStructure = Instantiate(_structure, _planetTransform, true);
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
                        _hoverStructure.transform.SetPositionAndRotation(_raycastHitPoint, rotation);

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            _structures.Add(_hoverStructure);
                            _hoverStructure = null;
                        }
                    }
                    else
                    {
                        _hoverStructure = Instantiate(_structure, _planetTransform, true);
                        _hoverStructure.transform.SetPositionAndRotation(_raycastHitPoint, rotation);
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