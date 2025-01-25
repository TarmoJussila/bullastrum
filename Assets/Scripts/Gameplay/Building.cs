using System.Collections.Generic;
using UnityEngine;

namespace Bullastrum.Gameplay
{
    public class Building : MonoBehaviour
    {
        [Header("Models")]
        [SerializeField] private List<GameObject> _models = new List<GameObject>();
        
        [Header("Settings")]
        [SerializeField] private bool _randomizeModelOnAwake = true;

        private void Awake()
        {
            if (_randomizeModelOnAwake)
            {
                RandomizeModel();
            }
        }
        
        private void RandomizeModel()
        {
            int randomIndex = Random.Range(0, _models.Count);
            foreach (GameObject model in _models)
            {
                model.SetActive(false);
            }
            _models[randomIndex].SetActive(true);
        }
    }
}
