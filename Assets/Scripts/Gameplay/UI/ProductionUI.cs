using UnityEngine;

namespace Bullastrum.Gameplay.UI
{
    public class ProductionUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _productionText;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _animationDelay = 0.1f;

        private float _timer;
        
        private static readonly int Bounce = Animator.StringToHash("Bounce");
        
        private void OnEnable()
        {
            Initialize(GameController.Instance.Production, false);
            GameController.OnProductionChanged += OnProductionChanged;
        }
        
        private void OnDestroy()
        {
            GameController.OnProductionChanged -= OnProductionChanged;
        }

        private void Update()
        {
            if (_timer > 0f)
            {
                _timer -= Time.deltaTime;
            }
        }

        private void OnProductionChanged(int count)
        {
            Initialize(count, true);
        }
        
        private void Initialize(int count, bool showAnimation = true)
        {
            _productionText.text = count.ToString();
            if (showAnimation && _timer <= 0f)
            {
                _animator.ResetTrigger(Bounce);
                _animator.SetTrigger(Bounce);
                _timer = _animationDelay;
            }
        }
    }
}
