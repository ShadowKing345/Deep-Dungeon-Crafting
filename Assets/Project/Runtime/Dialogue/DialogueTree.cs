using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue Tree", menuName = "SO/Dialogue Tree")]
    public class DialogueTree : ScriptableObject
    {
        [SerializeField] private DialogueNode[] nodes;
        [SerializeField] private DialogueNode startingNode;

        public DialogueNode[] Nodes => nodes;
        public DialogueNode StartingNode => startingNode;
    }
}