using Crafting;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ui.Crafting
{
    public class RecipeEntry : MonoBehaviour, IPointerClickHandler
    {
        public CraftingController Controller { get; set; }
        [SerializeField] private Recipe recipe;
        public Recipe Recipe
        {
            set
            {
                recipe = value;
                UpdateUi();
            }
        }

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;

        private void UpdateUi()
        {
            image.sprite = recipe == null ? null : recipe.Result.Item.Icon;
            image.color = recipe == null ? Color.clear : Color.white;
            text.text = recipe == null ? string.Empty : recipe.name;
        }

        public void OnPointerClick(PointerEventData eventData) => Controller.UpdatePage(recipe);
    }
}