using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

namespace Ui.Statistics
{
    public class StatisticsEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private Transform subContent;

        public KeyValuePair<string, object> Init
        {
            set
            {
                nameText.text = value.Key;

                if (value.Value is IDictionary<string, object> dictionary)
                {
                    foreach (var kvPair in new SortedDictionary<string, object>(dictionary))
                    {
                        var sub = Instantiate(gameObject, subContent);
                        if (sub.TryGetComponent(out StatisticsEntry entry)) entry.Init = kvPair;
                    }
                }
                else
                {
                    valueText.enabled = true;
                    valueText.text = value.Value.ToString();
                }
            }
        }

        private void OnEnable()
        {
            GameObjectUtils.ClearChildren(subContent);
            valueText.enabled = false;
        }
    }
}