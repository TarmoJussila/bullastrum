using UnityEngine;

namespace Bullastrum.Gameplay.UI
{
    public class CurrencyUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _currencyText;
        [SerializeField] private TMPro.TextMeshProUGUI _revenueText;
        [SerializeField] private TMPro.TextMeshProUGUI _expensesText;
        [SerializeField] private TMPro.TextMeshProUGUI _profitText;
        [SerializeField] private Animator _animator;

        [Header("Settings")]
        [SerializeField] private Color _plusColor;
        [SerializeField] private Color _minusColor;
        
        private static readonly int Bounce = Animator.StringToHash("Bounce");
        
        private void OnEnable()
        {
            GameController.OnCurrencyChanged += OnCurrencyChanged;
            GameController.OnEconomyChanged += OnEconomyChanged;
        }

        private void OnDisable()
        {
            GameController.OnCurrencyChanged -= OnCurrencyChanged;
            GameController.OnEconomyChanged -= OnEconomyChanged;
        }

        private void Start()
        {
            Initialize(GameController.Instance.Currency, false);
            OnEconomyChanged(0, 0, 0, 0, 0);
        }

        private void OnCurrencyChanged(int count)
        {
            Initialize(count, true);
        }
        
        private void OnEconomyChanged(int revenue, int expenses, int profit, int baseProduction, int productionMultiplier)
        {
            _revenueText.text = (revenue >= 0 ? "+" : "") + revenue.ToString();
            _expensesText.text = "-" + expenses.ToString();
            _profitText.text = (profit >= 0 ? "+" : "") + profit.ToString();
            
            _revenueText.color = revenue >= 0 ? _plusColor : _minusColor;
            _expensesText.color = _minusColor;
            _profitText.color = profit >= 0 ? _plusColor : _minusColor;
        }
        
        private void Initialize(int count, bool showAnimation = true)
        {
            _currencyText.text = count.ToString();
            if (showAnimation)
            {
                _animator.ResetTrigger(Bounce);
                _animator.SetTrigger(Bounce);
            }
        }
    }
}
