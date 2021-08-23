using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue Node", menuName = "SO/Dialogue Node")]
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private string characterName;
        [SerializeField] private bool isYou;
        [SerializeField] private string optionName;
        [Multiline] [SerializeField] private string body;
        [SerializeField] private DialogueNode[] nextNodes;

        public string CharacterName => characterName;
        public bool IsYou => isYou;

        public string OptionName => string.IsNullOrEmpty(optionName) || string.IsNullOrWhiteSpace(optionName)
            ? name
            : optionName;
        public string Body => body;
        public DialogueNode[] NextNodes => nextNodes;
    }
}