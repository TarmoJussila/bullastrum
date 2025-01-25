using UnityEngine;

namespace Bullastrum.Gameplay.UI
{
    public class PopulationUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _populationText;
        [SerializeField] private Animator _animator;
        
        private static readonly int Bounce = Animator.StringToHash("Bounce");

        private void OnEnable()
        {
            Initialize(GameController.Instance.Population, false);
            GameController.OnPopulationChanged += OnPopulationChanged;
        }
        
        private void OnDestroy()
        {
            GameController.OnPopulationChanged -= OnPopulationChanged;
        }
        
        private void OnPopulationChanged(int count)
        {
            Initialize(count, true);
        }

        private void Initialize(int count, bool showAnimation = true)
        {
            _populationText.text = count.ToString();
            if (showAnimation)
            {
                _animator.ResetTrigger(Bounce);
                _animator.SetTrigger(Bounce);
            }
        }
    }
}
