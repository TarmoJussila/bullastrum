using System;
using Bullastrum.Utility;
using UnityEngine;

namespace Bullastrum.Gameplay
{
    public class GameController : Singleton<GameController>
    {
        public static Action<int> OnPopulationChanged;
        public static Action<int> OnProductionChanged;
        public static Action<int> OnCurrencyChanged;
        public static Action<int, int, int> OnEconomyChanged;
        
        public int Population => _population;
        public int Production => _production;
        public int Currency => _currency;
        
        [SerializeField] private int _population;
        [SerializeField] private int _production;
        [SerializeField] private int _currency;
        [SerializeField] private float _economyUpdateRate = 10f;

        private int _currencyRevenue;
        private int _currencyExpenses;
        private float _timer;
        
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _economyUpdateRate)
            {
                _timer = 0f;
                _currencyRevenue = _production;
                _currencyExpenses = _population;
                int profit = _currencyRevenue - _currencyExpenses;
                AddCurrency(profit);
                OnEconomyChanged?.Invoke(_currencyRevenue, _currencyExpenses, profit);
            }
        }

        public void AddPopulation()
        {
            _population++;
            OnPopulationChanged?.Invoke(_population);
            Log.Message("Population changed: " + _population);
        }

        public void AddProduction()
        {
            _production++;
            OnProductionChanged?.Invoke(_production);
            Log.Message("Production changed: " + _production);
        }

        public void AddCurrency(int amount)
        {
            _currency += amount;
            OnCurrencyChanged?.Invoke(_currency);
            Log.Message("Currency changed: " + _currency);
        }
    }
}