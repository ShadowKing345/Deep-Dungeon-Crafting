using Project.Runtime.Crafting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Runtime.Ui.Crafting
{
    public class RecipeEntry : Button
    {
        [SerializeField] private Recipe recipe;
        [SerializeField] private Image recipeImage;

        [SerializeField] private TextMeshProUGUI text;
        public CraftingController Controller { get; set; }

        public Recipe Recipe
        {
            set
            {
                recipe = value;
                UpdateUi();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            onClick.AddListener(() =>
            {
                if (recipe != null) Controller.UpdatePage(recipe);
            });
        }

        private void UpdateUi()
        {
            if (recipe == null)
            {
                recipeImage.sprite = null;
                recipeImage.color = Color.clear;
                text.text = string.Empty;
            }
            else
            {
                recipeImage.sprite = recipe.Result.Item.Icon;
                recipeImage.color = Color.white;
                text.text = recipe.name;
            }
        }
    }
}