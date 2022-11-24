using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.Editor.Dialogue
{
    public class DialogueSystemEditor : EditorWindow
    {
        [MenuItem("Window/UI Toolkit/DialogueSystemEditor")]
        public static void ShowExample()
        {
            DialogueSystemEditor wnd = GetWindow<DialogueSystemEditor>();
            wnd.titleContent = new GUIContent("DialogueSystemEditor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            VisualElement label = new Label("Hello World! From C#");
            root.Add(label);

        }
    }
}
