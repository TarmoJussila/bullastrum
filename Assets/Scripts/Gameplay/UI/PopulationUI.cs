using UnityEngine;

namespace Bullastrum.Gameplay.UI
{
    public class PopulationUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _populationText;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _animationDelay = 0.1f;
        
        private float _timer;
        
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
        
        private void Update()
        {
            if (_timer > 0f)
            {
                _timer -= Time.deltaTime;
            }
        }
        
        private void OnPopulationChanged(int count)
        {
            Initialize(count, true);
        }

        private void Initialize(int count, bool showAnimation = true)
        {
            _populationText.text = count.ToString();
            if (showAnimation && _timer <= 0f)
            {
                _animator.ResetTrigger(Bounce);
                _animator.SetTrigger(Bounce);
                _timer = _animationDelay;
            }
        }
    }
}
