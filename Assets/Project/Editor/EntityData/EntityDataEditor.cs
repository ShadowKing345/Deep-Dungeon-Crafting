using Project.Runtime.Entity.Animations;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Project.Editor.EntityData
{
    public class EntityDataEditor : EditorWindow
    {
        private TwoPaneSplitView splitView;
        private VisualElement left;
        private VisualElement right;

        [MenuItem("Tools/Entity Data Editor")]
        public static void ShowEntityDataEditorWindow()
        {
            GetWindow<EntityDataEditor>("Entity Data Editor");
        }

        public void CreateGUI()
        {
            AddToolbar();
            AddContent();
        }

        private void AddToolbar()
        {
            var toolbar = new Toolbar();
            rootVisualElement.Add(toolbar);
        }

        private void AddContent()
        {
            splitView = new TwoPaneSplitView(0, 500f, TwoPaneSplitViewOrientation.Horizontal);
            left = new VisualElement();
            right = new VisualElement();
            splitView.Add(left);
            splitView.Add(right);

            AddLeft();
            AddRight();

            rootVisualElement.Add(splitView);
        }

        private void AddLeft()
        {
            AddDescription();

            var centerPos = new Vector2Field { label = "Center Position" };
            left.Add(centerPos);
        }

        private void AddRight()
        {
            var vSplit = new TwoPaneSplitView(1, 250f, TwoPaneSplitViewOrientation.Vertical);

            var top = new VisualElement();
            var bottom = new VisualElement();

            vSplit.Add(top);
            vSplit.Add(bottom);

            bottom.Add(new EdeAnimationClipEditor(new EntityAnimation()));
            bottom.Add(new EdeAnimationClipEditor(new EntityAnimation()));

            right.Add(vSplit);
        }

        private void AddDescription()
        {
            var descriptionField = new TextField { label = "Description" };
            var maxHealthField = new FloatField { label = "Max Health" };
            var maxManaField = new FloatField { label = "Max Mana" };

            left.Add(descriptionField);
            left.Add(maxHealthField);
            left.Add(maxManaField);
        }
    }
}