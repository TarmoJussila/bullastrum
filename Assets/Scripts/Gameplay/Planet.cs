using UnityEngine;

namespace Bullastrum.Gameplay
{
    public class Planet : MonoBehaviour
    {
        public Vector3 CenterPoint { get { return transform.position; } }
        
        [SerializeField] private float _rotationSpeed = 100f;

        private Vector3 _lastMousePosition;
        private bool _isDragging = false;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                _isDragging = true;
                _lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                _isDragging = false;
            }

            if (_isDragging)
            {
                RotatePlanet();
            }
        }

        private void RotatePlanet()
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - _lastMousePosition;
            
            float rotationX = -mouseDelta.y * _rotationSpeed * Time.deltaTime;
            float rotationY = mouseDelta.x * _rotationSpeed * Time.deltaTime;
            
            transform.Rotate(rotationX, rotationY, 0, Space.World);
            
            _lastMousePosition = currentMousePosition;
        }
    }
}