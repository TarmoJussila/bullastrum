using UnityEngine;

namespace Bullastrum.Gameplay.UI
{
    public class ProductionUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _productionText;
        [SerializeField] private Animator _animator;
        
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
        
        private void OnProductionChanged(int count)
        {
            Initialize(count, true);
        }
        
        private void Initialize(int count, bool showAnimation = true)
        {
            _productionText.text = count.ToString();
            if (showAnimation)
            {
                _animator.ResetTrigger(Bounce);
                _animator.SetTrigger(Bounce);
            }
        }
    }
}
