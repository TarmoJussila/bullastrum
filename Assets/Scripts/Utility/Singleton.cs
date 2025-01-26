using UnityEngine;

namespace Bullastrum.Utility
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private const bool DontDestroyOnLoadEnabled = false;

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (!DontDestroyOnLoadEnabled)
            {
                return;
            }
            
            if (_instance == null)
            {
                _instance = this as T;

                bool isRoot = (transform.root == transform) ? true : false;
                if (isRoot)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}