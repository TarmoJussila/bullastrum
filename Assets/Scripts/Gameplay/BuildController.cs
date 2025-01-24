using System.Collections.Generic;
using Bullastrum.Utility;
using UnityEngine;
using ColorUtility = Bullastrum.Utility.ColorUtility;

namespace Bullastrum.Gameplay
{
    public class BuildController : Singleton<BuildController>
    {
        [Header("Raycast")]
        public LayerMask PlanetLayerMask;
        public float RaycastMaxDistance;

        private Ray ray;
        private RaycastHit raycastHit;
        private Vector3 raycastHitPoint;

        [Header("Prefabs")]
        public GameObject Structure;

        [Header("Debug")]
        public bool BuildingEnabled = true;
        public float RaycastHitPointRadius = 1.0f;
        public ColorUtility.Color RaycastHitPointColor;

        private GameObject _hoverStructure;
        private List<GameObject> _structures = new List<GameObject>();

        private void Start()
        {
            _hoverStructure = Instantiate(Structure);
        }

        private void Update()
        {
            if (BuildingEnabled)
            {
                ray = CameraController.Instance.Camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out raycastHit, RaycastMaxDistance, PlanetLayerMask))
                {
                    raycastHitPoint = raycastHit.point;
                    Vector3 relativePosition = raycastHitPoint - raycastHit.transform.position;
                    Quaternion rotation = Quaternion.LookRotation(relativePosition);

                    if (_hoverStructure != null)
                    {
                        _hoverStructure.transform.position = raycastHitPoint;
                        _hoverStructure.transform.rotation = rotation;

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            _structures.Add(_hoverStructure);
                            _hoverStructure = null;
                        }
                    }
                    else
                    {
                        _hoverStructure = Instantiate(Structure, raycastHitPoint, rotation, transform);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (raycastHitPoint != Vector3.zero)
            {
                Gizmos.color = ColorUtility.GetColor(RaycastHitPointColor);
                Gizmos.DrawSphere(raycastHitPoint, RaycastHitPointRadius);
            }
        }
    }
}