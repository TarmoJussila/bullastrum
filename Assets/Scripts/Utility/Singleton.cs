using UnityEngine;

namespace Bullastrum.Utility
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            { if (_instance == null)
              {
                  _instance = FindObjectOfType<T>();
              }
              return _instance; }
        }

        protected virtual void Reset()
        {
            var instances = FindObjectsOfType<T>();

            if (instances != null && instances.Length > 1)
            {
                Log.Message(GetType().ToString(), "Singleton already exists.", ColorUtility.Color.Red, Log.Type.Error);
                DestroyImmediate(this);
            }
            else
            {
                var components = GetComponents<Component>();

                if (components != null && components.Length <= 2)
                {
                    transform.localPosition = Vector3.zero;
                    name = GetType().ToString();
                }
            }
        }

        protected virtual void Awake()
        {
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