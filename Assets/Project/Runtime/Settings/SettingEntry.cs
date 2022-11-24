using System;
using Project.Runtime.Utils;
using UnityEngine;

namespace Project.Runtime.Settings
{
    [Serializable]
    public class SettingEntry
    {
        [SerializeField] private string name;
        [SerializeField] private SettingEntryValueType type;

        [SerializeField] private bool toggleValue;

        [SerializeField] private MinMax<float> rangeRanges;
        [SerializeField] private float rangeValue;

        [SerializeField] private string[] selection;
        [SerializeField] private int selectionValue;

        public string Name => name;
        public SettingEntryValueType Type => type;

        public bool ToggleValue => toggleValue;

        public MinMax<float> RangeRanges => rangeRanges;
        public float RangeValue => rangeValue;

        public string[] Selection => selection;
        public int SelectionValue => selectionValue;

        public object Value => type switch
        {
            SettingEntryValueType.Toggle => toggleValue,
            SettingEntryValueType.Range => rangeValue,
            SettingEntryValueType.Selection => selectionValue,
            _ => null
        };
    }
}