using Combat;
using Items;
using Managers;
using Ui.ToolTip.Types;
using UnityEngine;

namespace Ui.ToolTip
{
    public class ToolTipSystem : MonoBehaviour
    {
        private static ToolTipSystem _instance;
        public static ToolTipSystem Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<ToolTipSystem>();
                return _instance;
            }
            private set
            {
                if (_instance != null && _instance != value)
                {
                    Destroy(value.gameObject);
                    return;
                }

                _instance = value;
            }
        }

        private InputManager _inputManager;

        [Header("Toll Tip Game Objects")]
        [SerializeField] private ItemToolTip itemToolTip;
        [SerializeField] private AbilityToolTip abilityToolTip;
        [SerializeField] private TextToolTip textToolTip;
        [Space] 
        [SerializeField] private bool hideAdvanceToolTips;

        public bool HideAdvanceToolTips => hideAdvanceToolTips;

        private void OnEnable()
        {
            Instance ??= this;
            _inputManager ??= new InputManager();

            _inputManager.ToolTip.Enable();
            _inputManager.ToolTip.HideAdvancedToolTips.started += _ => hideAdvanceToolTips = true;
            _inputManager.ToolTip.HideAdvancedToolTips.canceled += _ => hideAdvanceToolTips = false;
        }

        private void OnDisable() => _inputManager.ToolTip.Disable();

        public void ShowToolTIp(ItemStack stack)
        {
            itemToolTip.gameObject.SetActive(true);
            itemToolTip.ItemStack = stack;
        }
        
        public void ShowToolTip(Ability ability)
        {
            abilityToolTip.gameObject.SetActive(true);
            abilityToolTip.Ability = ability;
        }

        public void ShowToolTip(string content, string header = "")
        {
            textToolTip.gameObject.SetActive(true);
            textToolTip.UpdateContent(content, header);
            LeanTween.alphaCanvas(textToolTip.canvasGroup, 1, 0.1f);
        }

        public void HideToolTip(bool normal = false, bool item = false, bool ability = false)
        {
            if (normal) textToolTip.gameObject.SetActive(false);
            if (item) itemToolTip.gameObject.SetActive(false);
            if (ability) abilityToolTip.gameObject.SetActive(false);
        }
    }
}