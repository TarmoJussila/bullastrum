using System;
using Bullastrum.Gameplay.UI;
using Bullastrum.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bullastrum.Gameplay
{
    public class GameController : Singleton<GameController>
    {
        public static Action<int> OnPopulationChanged;
        public static Action<int> OnProductionChanged;
        public static Action<int, bool> OnCurrencyChanged;
        public static Action<int, int, int, int, int> OnEconomyChanged;
        
        public int Population => _population;
        public int Production => _production;
        public int Currency => _currency;
        public int PopulationBuildCost => Mathf.RoundToInt(((Population * BuildCostIncreaseMultiplier) + 1) * BaseBuildCost);
        public int ProductionBuildCost => Mathf.RoundToInt(((Production * BuildCostIncreaseMultiplier) + 1) * BaseBuildCost);
        public bool GameActive => _gameActive;
        
        [SerializeField] private int _population;
        [SerializeField] private int _production;
        [SerializeField] private int _currency;
        [SerializeField] private float _economyUpdateRate = 10f;
        [SerializeField] private int _startingCurrency = 1000;

        private int _currencyRevenue;
        private int _currencyExpenses;
        private float _timer;
        private bool _gameActive = false;

        private const int BaseBuildCost = 100;
        private const float BuildCostIncreaseMultiplier = 0.25f;
        private const float PopulationProductionMultiplier = 0.5f;

        private void Start()
        {
            ResetGame();
        }

        private void Update()
        {
            if (!_gameActive)
            {
                return;
            }
            
            _timer += Time.deltaTime;
            if (_timer >= _economyUpdateRate)
            {
                _timer = 0f;
                int productionMultiplier = Mathf.CeilToInt(_population * PopulationProductionMultiplier);
                int baseProduction = _production;
                _currencyRevenue = _production * productionMultiplier;
                _currencyExpenses = _population;
                int profit = _currencyRevenue - _currencyExpenses;
                AddCurrency(profit, true);
                OnEconomyChanged?.Invoke(_currencyRevenue, _currencyExpenses, profit, baseProduction, productionMultiplier);

                if (_currency < 0)
                {
                    Log.Message("Game Over");
                    UIController.Instance.ShowGameOverUI();
                }
            }
        }

        public void AddPopulation()
        {
            _population++;
            OnPopulationChanged?.Invoke(_population);
            Log.Message("Population changed: " + _population);
        }

        public void RemovePopulation()
        {
            if (_population > 0)
            {
                _population--;
                OnPopulationChanged?.Invoke(_population);
                Log.Message("Population removed: " + _population);
            }
        }

        public void AddProduction()
        {
            _production++;
            OnProductionChanged?.Invoke(_production);
            Log.Message("Production changed: " + _production);
        }
        
        public void RemoveProduction()
        {
            if (_production > 0)
            {
                _production--;
                OnProductionChanged?.Invoke(_production);
                Log.Message("Production removed: " + _production);
            }
        }

        public void AddCurrency(int amount, bool showAnimation = false)
        {
            _currency += amount;
            OnCurrencyChanged?.Invoke(_currency, showAnimation);
            Log.Message("Currency changed: " + _currency);
        }

        public void RemoveCurrency(int amount)
        {
            AddCurrency(-amount);
        }

        public void StartGame()
        {
            _gameActive = true;
        }

        public void PauseGame()
        {
            _gameActive = false;
        }

        public void ResetGame(bool reloadScene = false)
        {
            PauseGame();
            _population = 0;
            _production = 0;
            _currency = _startingCurrency;
            _currencyRevenue = 0;
            _currencyExpenses = 0;
            _timer = 0;

            if (reloadScene)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}