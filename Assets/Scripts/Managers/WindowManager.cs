using Items;
using Ui;
using UnityEngine;

namespace Managers
{
    public class WindowManager : MonoBehaviour
    {
        public static WindowManager instance;

        public Canvas canvas;
        
        public ProgressBar healthProgressBar;
        public ProgressBar manaProgressBar;

        public WeaponAbilityUi action1;
        public WeaponAbilityUi action2;
        public WeaponAbilityUi action3;

        public GameObject itemHoverObj;

        private void OnEnable()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void Awake()
        {
            canvas ??= FindObjectOfType<Canvas>();
            
            healthProgressBar ??= GameObject.Find("HealthProgressBar").GetComponent<ProgressBar>();
            manaProgressBar ??= GameObject.Find("ManaProgressBar").GetComponent<ProgressBar>();

            action1 ??= GameObject.Find("ActionUi1").GetComponent<WeaponAbilityUi>();
            action2 ??= GameObject.Find("ActionUi2").GetComponent<WeaponAbilityUi>();
            action3 ??= GameObject.Find("ActionUi3").GetComponent<WeaponAbilityUi>();
        }

        public void BeginItemHover(GameObject preFab, ItemStack stack)
        {
            if (itemHoverObj != null) EndItemHover();
            
            itemHoverObj = Instantiate(preFab, canvas.transform);
            itemHoverObj.GetComponent<HoverItem>().Init(stack, canvas);
        }

        public void EndItemHover()
        {
            if (itemHoverObj == null) return;
            Destroy(itemHoverObj);
            itemHoverObj = null;
        }
    }
}