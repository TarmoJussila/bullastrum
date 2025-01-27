using UnityEngine;

namespace Bullastrum.Gameplay.UI
{
    public class WorldText : MonoBehaviour
    {
        public bool IsActive => gameObject.activeSelf;
        
        [Header("References")]
        [SerializeField] private TMPro.TextMeshProUGUI _text;
        [SerializeField] private Animator _animator;
        
        [Header("Settings")]
        [SerializeField] private float _lifeTime = 1.0f;

        private float _timer;
        private bool _isInitialized = false;
        
        private static readonly int Show = Animator.StringToHash("Show");

        public void Initialize(string text, Vector3 position, Color color)
        {
            _text.text = text;
            _text.color = color;
            _animator.ResetTrigger(Show);
            _animator.SetTrigger(Show);
            _timer = _lifeTime;
            _isInitialized = true;
            transform.position = position;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!_isInitialized)
            {
                return;
            }
            
            if (_timer > 0f)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                _isInitialized = false;
                gameObject.SetActive(false);
            }
        }
    }
}
