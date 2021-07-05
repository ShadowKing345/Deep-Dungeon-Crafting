using Entity.Player;
using Interfaces;
using UnityEngine;

namespace Ui.Menu
{
    public class HelpMenu : MonoBehaviour, IUiElement
    {
        public void Show() => Time.timeScale = 0;
        public void Hide() => Time.timeScale = 1;
    }
}