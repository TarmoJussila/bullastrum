using Bullastrum.Utility;
using UnityEngine;

namespace Bullastrum.Gameplay.UI
{
    public class UIController : Singleton<UIController>
    {
        [SerializeField] private GameObject _menuUI;
        [SerializeField] private GameObject _gameUI;
        [SerializeField] private GameObject _gameOverUI;

        private void Awake()
        {
            ShowMenuUI();
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
    }
}
