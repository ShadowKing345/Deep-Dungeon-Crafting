using Managers;
using UnityEngine;

namespace Ui.Menu
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private HudElement hudElement;
        private UiManager uiManager;

        private void Awake()
        {
            // uiManager = UiManager.Instance;
            // uiManager.SetUpHud(hudElement);
        }

        // private void OnDestroy() => uiManager.DestroyHud();
    }
}