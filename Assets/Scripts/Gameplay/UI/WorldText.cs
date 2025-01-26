using UnityEngine;

namespace Bullastrum.Gameplay.UI
{
    public class WorldText : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _text;
        [SerializeField] private Animator _animator;
        
        private static readonly int Show = Animator.StringToHash("Show");

        public void Initialize(string text, Color color)
        {
            _text.text = text;
            _text.color = color;
            _animator.SetTrigger(Show);
        }
    }
}
