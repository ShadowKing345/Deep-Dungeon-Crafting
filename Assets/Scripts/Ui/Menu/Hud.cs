using Managers;
using UnityEngine;

namespace Ui.Menu
{
    public class Hud : MonoBehaviour
    {
        private UiManager _uiManager;

        [SerializeField] private HudElement hudElement;
        
        private void Awake()
        {
            _uiManager = UiManager.Instance;
            _uiManager.SetUpHud(hudElement);
        }

        private void OnDestroy() => _uiManager.DestroyHud();
    }
}