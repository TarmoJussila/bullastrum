using Bullastrum.Utility;
using UnityEngine;

namespace Bullastrum.Gameplay
{
    public class CameraController : Singleton<CameraController>
    {
        public Camera Camera => _camera;
        
        [SerializeField] private Camera _camera;
    }
}