using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    [ExecuteInEditMode]
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private float _current;
        [SerializeField] private float _max = 100f;
        
        public float Current { get => _current; set => _current = value; }
        public float MAX { get => _max; set => _max = value; }
    
        [SerializeField] private Image mask;
        private bool IsMaskNull => mask == null;

        private void Start() => mask ??= GameObject.Find("Mask").GetComponent<Image>();

        protected void Update()
        {
            GetFill();
        }

        private void GetFill()
        {
            if (IsMaskNull) return;
            mask.fillAmount = _current / _max;
        }
    }
}
