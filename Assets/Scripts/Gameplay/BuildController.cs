using System.Collections.Generic;
using Bullastrum.Utility;
using UnityEngine;
using ColorUtility = Bullastrum.Utility.ColorUtility;

namespace Bullastrum.Gameplay
{
    public enum BuildingType
    {
        Demolish = -1,
        PopulationBuilding = 0,
        ProductionBuilding = 1,
    }
    
    public class BuildController : Singleton<BuildController>
    {
        [Header("Raycast")]
        [SerializeField] private LayerMask _planetLayerMask;
        [SerializeField] private LayerMask _buildingLayerMask;
        [SerializeField] private float _raycastMaxDistance;
        [SerializeField] private Transform _planetTransform;
        
        [Header("Prefabs")]
        [SerializeField] private Building _populationBuildingPrefab;
        [SerializeField] private Building _productionBuildingPrefab;
        [SerializeField] private GameObject _demolishPrefab;
        
        [Header("Debug")]
        [SerializeField] private bool _buildingEnabled = true;
        [SerializeField] private float _raycastHitPointRadius = 1.0f;
        [SerializeField] private ColorUtility.Color _raycastHitPointColor;

        private Ray _ray;
        private RaycastHit _raycastHit;
        private Vector3 _raycastHitPoint;
        private Building _currentBuilding;
        private GameObject _demolishObject;
        private readonly List<Building> _buildings = new List<Building>();
        private BuildingType _buildingType;
        private bool _demolishMode;

        private void Start()
        {
            _demolishMode = false;
            _planetTransform = FindFirstObjectByType<Planet>().transform;
            _currentBuilding = Instantiate(GetCurrentBuildingPrefab(), _planetTransform, true);
            _currentBuilding.SetColliderEnabled(false);
        }

        private void Update()
        {
            if (!GameController.Instance.GameActive)
            {
                return;
            }
            
            if (_buildingEnabled)
            {
                _ray = CameraController.Instance.Camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(_ray, out _raycastHit, _raycastMaxDistance, _planetLayerMask))
                {
                    _raycastHitPoint = _raycastHit.point;
                    Vector3 relativePosition = _raycastHitPoint - _raycastHit.transform.position;
                    Quaternion rotation = Quaternion.LookRotation(relativePosition);

                    if (_demolishMode)
                    {
                        if (_demolishObject != null)
                        {
                            _demolishObject.transform.SetPositionAndRotation(_raycastHitPoint, rotation);
                        }
                        
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            if (Physics.Raycast(_ray, out _raycastHit, _raycastMaxDistance, _buildingLayerMask))
                            {
                                var building = _raycastHit.transform.GetComponent<Building>();
                                if (building != null)
                                {
                                    int demolishCost = GetBuildingDemolishCost(building.BuildingType);
                                    GameController.Instance.AddCurrency(demolishCost);
                                    Destroy(building.gameObject);
                                }
                                else
                                {
                                    Log.Message("Cannot demolish non-building object!");
                                }
                            }
                        }
                        return;
                    }

                    if (_currentBuilding != null)
                    {
                        _currentBuilding.transform.SetPositionAndRotation(_raycastHitPoint, rotation);

                        if (GameController.Instance.Currency < GetCurrentBuildingCost())
                        {
                            Log.Message("Not enough currency to build: " + _buildingType);
                            return;
                        }
                        
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            if (Physics.Raycast(_ray, out _raycastHit, _raycastMaxDistance, _buildingLayerMask))
                            {
                                Log.Message("Cannot build on top of another building");
                                return;
                            }
                            
                            _buildings.Add(_currentBuilding);
                            _currentBuilding.SetColliderEnabled(true);
                            _currentBuilding = null;
                            NotifyBuildingPlaced();
                        }
                    }
                    else
                    {
                        _currentBuilding = Instantiate(GetCurrentBuildingPrefab(), _planetTransform, true);
                        _currentBuilding.transform.SetPositionAndRotation(_raycastHitPoint, rotation);
                        _currentBuilding.SetColliderEnabled(false);
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

        private void NotifyBuildingPlaced()
        {
            Log.Message("Building placed: " + _buildingType + " | Buildings: " + _buildings.Count);
            if (_buildingType == BuildingType.PopulationBuilding)
            {
                GameController.Instance.RemoveCurrency(GameController.Instance.PopulationBuildCost);
                GameController.Instance.AddPopulation();
            }
            else if (_buildingType == BuildingType.ProductionBuilding)
            {
                GameController.Instance.RemoveCurrency(GameController.Instance.ProductionBuildCost);
                GameController.Instance.AddProduction();
            }
            AudioPlayer.Instance.Play();
        }

        private int GetCurrentBuildingCost()
        {
            if (_buildingType == BuildingType.PopulationBuilding)
            {
                return GameController.Instance.PopulationBuildCost;
            }
            else if (_buildingType == BuildingType.ProductionBuilding)
            {
                return GameController.Instance.ProductionBuildCost;
            }
            return int.MaxValue;
        }
        
        private int GetBuildingDemolishCost(BuildingType buildingType)
        {
            if (buildingType == BuildingType.PopulationBuilding)
            {
                return Mathf.RoundToInt(GameController.Instance.PopulationBuildCost / 2f);
            }
            else if (buildingType == BuildingType.ProductionBuilding)
            {
                return Mathf.RoundToInt(GameController.Instance.ProductionBuildCost / 2f);
            }
            return 0;
        }

        private Building GetCurrentBuildingPrefab()
        {
            if (_buildingType == BuildingType.PopulationBuilding)
            {
                return _populationBuildingPrefab;
            }
            else if (_buildingType == BuildingType.ProductionBuilding)
            {
                return _productionBuildingPrefab;
            }
            return null;
        }

        public void SetBuildingType(BuildingType buildingType)
        {
            bool buildingTypeChanged = _buildingType != buildingType;
            
            _buildingType = buildingType;
            Log.Message("Set building type: " + _buildingType);
            
            if (buildingTypeChanged)
            {
                if (_currentBuilding != null)
                {
                    Destroy(_currentBuilding.gameObject);
                    _currentBuilding = null;
                }
                _currentBuilding = Instantiate(GetCurrentBuildingPrefab(), _planetTransform, true);
                _currentBuilding.SetColliderEnabled(false);
            }
            DisableDemolishMode();
        }

        public void SetDemolishMode()
        {
            if (_currentBuilding != null)
            {
                Destroy(_currentBuilding.gameObject);
                _currentBuilding = null;
            }
            _demolishObject = Instantiate(_demolishPrefab, _planetTransform, true);
            _demolishMode = true;
        }

        private void DisableDemolishMode()
        {
            _demolishMode = false;
            if (_demolishObject != null)
            {
                Destroy(_demolishObject);
            }
        }

        public void SetBuildingType(int buildingType)
        {
            SetBuildingType((BuildingType)buildingType);
        }
    }
}