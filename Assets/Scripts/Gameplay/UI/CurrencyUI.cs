using UnityEngine;

namespace Bullastrum.Gameplay.UI
{
    public class CurrencyUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _currencyText;
        [SerializeField] private TMPro.TextMeshProUGUI _incomeText;
        [SerializeField] private TMPro.TextMeshProUGUI _outcomeText;
        [SerializeField] private Animator _animator;
        
        private static readonly int Bounce = Animator.StringToHash("Bounce");
        
        private void OnEnable()
        {
            Initialize(GameController.Instance.Currency, false);
            GameController.OnCurrencyChanged += OnCurrencyChanged;
            GameController.OnCurrencyRateChanged += OnCurrencyRateChanged;
        }

        private void OnDestroy()
        {
            GameController.OnCurrencyChanged -= OnCurrencyChanged;
            GameController.OnCurrencyRateChanged -= OnCurrencyRateChanged;
        }
        
        private void OnCurrencyChanged(int count)
        {
            Initialize(count, true);
        }
        
        private void OnCurrencyRateChanged(int income, int outcome)
        {
            _incomeText.text = (income >= 0 ? "+" : "-") + income.ToString();
            _outcomeText.text = "-" + outcome.ToString();
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
