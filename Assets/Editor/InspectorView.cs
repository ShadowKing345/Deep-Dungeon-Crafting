using System.Security.Policy;
using UnityEngine.UIElements;

namespace Editor
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory: UxmlFactory<InspectorView, UxmlTraits> {}
        
        public InspectorView()
        {
        }
    }
}