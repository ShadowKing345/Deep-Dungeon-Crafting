using UnityEditor;
using UnityEngine.UIElements;

namespace Project.Editor.EntityData
{
    public class EntityDataEditor : EditorWindow
    {
        [MenuItem("Tools/Entity Data Editor")]
        public static void ShowExample()
        {
            GetWindow<EntityDataEditor>("Entity Data Editor");
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;

            var left = new VisualElement();
            var right = new VisualElement();

            var descriptionLabel = new Label("Description");
            var descriptionField = new TextField();
            var maxHealthLabel = new Label("Max Health");
            var maxHealthField = new IntegerField();
            var maxManaLabel = new Label("Max Mana");
            var maxManaField = new IntegerField();
            
            left.Add(descriptionLabel);
            left.Add(descriptionField);
            left.Add(maxHealthLabel);
            right.Add(maxHealthField);
            right.Add(maxManaLabel);
            right.Add(maxManaField);

            root.Add(left);
            root.Add(right);
        }
    }
}