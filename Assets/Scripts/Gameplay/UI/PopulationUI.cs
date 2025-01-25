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
            GameController.OnPopulationChanged += OnPopulationChanged;
        }
        
        private void OnDestroy()
        {
            GameController.OnPopulationChanged -= OnPopulationChanged;
        }
        
        private void OnPopulationChanged(int count)
        {
            _populationText.text = count.ToString();
            _animator.SetTrigger(Bounce);
        }
    }
}
