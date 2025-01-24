using Bullastrum.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bullastrum.Gameplay
{
    public class CameraController : Singleton<CameraController>
    {
        public Camera Camera => camera;

        [FormerlySerializedAs("Camera")]
        [SerializeField] private Camera camera;
    }
}