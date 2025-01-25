using UnityEngine;
using UnityEngine.Serialization;

namespace Bullastrum.Gameplay.UI
{
    public class ProductionUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _productionText;
        [SerializeField] private Animator _animator;
        
        private static readonly int Bounce = Animator.StringToHash("Bounce");
        
        private void OnEnable()
        {
            GameController.OnProductionChanged += OnProductionChanged;
        }
        
        private void OnDestroy()
        {
            GameController.OnProductionChanged -= OnProductionChanged;
        }
        
        private void OnProductionChanged(int count)
        {
            _productionText.text = count.ToString();
            _animator.SetTrigger(Bounce);
        }
    }
}
