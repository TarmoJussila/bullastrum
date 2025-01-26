using UnityEngine;

namespace Bullastrum.Gameplay.UI
{
    public class ProductionUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _productionText;
        [SerializeField] private TMPro.TextMeshProUGUI _baseProductionText;
        [SerializeField] private TMPro.TextMeshProUGUI _productionMultiplierText;
        [SerializeField] private TMPro.TextMeshProUGUI _totalProductionText;
        [SerializeField] private TMPro.TextMeshProUGUI _productionBuildCostText;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _animationDelay = 0.1f;

        private float _timer;
        private int _productionMultiplier;
        private int _baseProduction;
        
        private static readonly int Bounce = Animator.StringToHash("Bounce");
        
        private void OnEnable()
        {
            GameController.OnProductionChanged += OnProductionChanged;
            GameController.OnEconomyChanged += OnEconomyChanged;
        }
        
        private void OnDisable()
        {
            GameController.OnProductionChanged -= OnProductionChanged;
            GameController.OnEconomyChanged += OnEconomyChanged;
        }

        private void Start()
        {
            Initialize(GameController.Instance.Production, false);
            OnEconomyChanged(0, 0, 0, 0, 0);
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
        
        private void OnEconomyChanged(int revenue, int expenses, int profit, int baseProduction, int productionMultiplier)
        {
            if (_productionMultiplier != productionMultiplier)
            {
                Initialize(baseProduction, baseProduction > 0);
            }
            else if (_baseProduction != baseProduction)
            {
                Initialize(baseProduction, baseProduction > 0);
            }
            
            _productionMultiplier = productionMultiplier;
            _baseProduction = baseProduction;
            _baseProductionText.text = baseProduction.ToString();
            _productionMultiplierText.text = "x " + productionMultiplier.ToString();
            _totalProductionText.text = (baseProduction * productionMultiplier).ToString();
        }
        
        private void Initialize(int count, bool showAnimation = true)
        {
            _productionText.text = (count * _productionMultiplier).ToString();
            _productionBuildCostText.text = GameController.Instance.ProductionBuildCost.ToString();
            if (showAnimation && _timer <= 0f)
            {
                _animator.ResetTrigger(Bounce);
                _animator.SetTrigger(Bounce);
                _timer = _animationDelay;
            }
        }
    }
}
