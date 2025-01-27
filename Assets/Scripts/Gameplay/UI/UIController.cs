using System.Collections.Generic;
using Bullastrum.Utility;
using UnityEngine;

namespace Bullastrum.Gameplay.UI
{
    public class UIController : Singleton<UIController>
    {
        [Header("References")]
        [SerializeField] private GameObject _menuUI;
        [SerializeField] private GameObject _gameUI;
        [SerializeField] private GameObject _gameOverUI;
        
        [Header("Prefabs")]
        [SerializeField] private WorldText _worldTextPrefab;
        
        [Header("Settings")]
        [SerializeField] private float _worldTextDelay = 0.5f;
        
        private readonly List<WorldText> _worldTextPool = new List<WorldText>();
        private float _timer;

        protected override void Awake()
        {
            base.Awake();
            ShowMenuUI();
        }

        private void Update()
        {
            if (_timer > 0f)
            {
                _timer -= Time.deltaTime;
            }
        }

        public void ShowMenuUI()
        {
            _menuUI.SetActive(true);
            _gameUI.SetActive(false);
            _gameOverUI.SetActive(false);
        }
        
        public void ShowGameUI()
        {
            _menuUI.SetActive(false);
            _gameUI.SetActive(true);
            _gameOverUI.SetActive(false);
        }
        
        public void ShowGameOverUI()
        {
            _menuUI.SetActive(false);
            _gameUI.SetActive(false);
            _gameOverUI.SetActive(true);
        }
        
        public void ShowWorldText(string text, Vector3 position, Color color, bool skipCooldown = false)
        {
            if (_timer <= 0f || skipCooldown)
            {
                var worldTextObject = _worldTextPool.Find(x => x.IsActive == false);
                if (worldTextObject == null)
                {
                    worldTextObject = Instantiate(_worldTextPrefab, position, Quaternion.identity);
                    _worldTextPool.Add(worldTextObject);
                }
                worldTextObject.Initialize(text, position, color);
                _timer = _worldTextDelay;
            }
        }
    }
}
