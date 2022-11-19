using Managers;
using UnityEngine;

namespace Ui.Menu
{
    public class Hud : MonoBehaviour
    {
        private UiManager uiManager;

        [SerializeField] private HudElement hudElement;
        
        private void Awake()
        {
            // uiManager = UiManager.Instance;
            // uiManager.SetUpHud(hudElement);
        }

        // private void OnDestroy() => uiManager.DestroyHud();
    }
}