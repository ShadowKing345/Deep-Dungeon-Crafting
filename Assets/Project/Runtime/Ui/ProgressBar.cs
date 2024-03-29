using UnityEngine;
using UnityEngine.UI;

namespace Project.Runtime.Ui
{
    [ExecuteInEditMode]
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private float current;
        [SerializeField] private float max = 100f;
        [Space] [SerializeField] private Image mask;

        public float Current
        {
            get => current;
            set => current = value;
        }

        public float Max
        {
            get => max;
            set => max = value;
        }

        private bool IsMaskNull => mask == null;

        protected void Update()
        {
            if (IsMaskNull) return;
            mask.fillAmount = current / max;
        }
    }
}