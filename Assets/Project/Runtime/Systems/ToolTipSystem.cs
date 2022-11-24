using Entity.Combat.Abilities;
using Items;
using Ui.Inventories;
using Ui.ToolTip.Types;
using UnityEngine;
using Inputs;

namespace Systems
{
    public class ToolTipSystem : MonoBehaviour
    {
        private static ToolTipSystem _instance;

        [Header("Toll Tip Game Objects")] [SerializeField]
        private ItemToolTip itemToolTip;

        [SerializeField] private AbilityToolTip abilityToolTip;
        [SerializeField] private TextToolTip textToolTip;

        [Space] [SerializeField] private bool hideAdvanceToolTips;

        [Space] [Header("Hover Item Stack")] [SerializeField]
        private HoverItem hoverItem;

        private InputManager _inputManager;

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


        public bool HideAdvanceToolTips => hideAdvanceToolTips;

        private void OnEnable()
        {
            Instance ??= this;
            _inputManager ??= new InputManager();

            // _inputManager.ToolTip.Enable();
            // _inputManager.ToolTip.HideAdvancedToolTips.started += _ => hideAdvanceToolTips = true;
            // _inputManager.ToolTip.HideAdvancedToolTips.canceled += _ => hideAdvanceToolTips = false;
        }

        private void OnDisable()
        {
            // _inputManager.ToolTip.Disable();
        }

        public void ShowToolTIp(ItemStack stack)
        {
            itemToolTip.gameObject.SetActive(true);
            itemToolTip.ItemStack = stack;
        }

        public void ShowToolTip(AbilityBase abilityBase)
        {
            abilityToolTip.gameObject.SetActive(true);
            abilityToolTip.AbilityBase = abilityBase;
        }

        public void ShowToolTip(string content, string header = "")
        {
            textToolTip.gameObject.SetActive(true);
            textToolTip.UpdateContent(content, header);
        }

        public void HideToolTip(bool normal = false, bool item = false, bool ability = false)
        {
            if (normal) textToolTip.gameObject.SetActive(false);
            if (item) itemToolTip.gameObject.SetActive(false);
            if (ability) abilityToolTip.gameObject.SetActive(false);
        }

        public void BeginItemHover(ItemStack stack)
        {
            if (!hoverItem.Stack.IsEmpty) EndItemHover();
            hoverItem.gameObject.SetActive(true);
            hoverItem.Stack = stack;
        }

        public void EndItemHover()
        {
            hoverItem.Stack = ItemStack.Empty;
            hoverItem.gameObject.SetActive(false);
        }
    }
}