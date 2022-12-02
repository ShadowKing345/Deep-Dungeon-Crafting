using System;
using System.Linq;
using Project.Runtime.Managers;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Runtime.Dialogue
{
    public class DialogueTrigger : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueTree tree;

        [SerializeField] private UnityEvent onDialogueFinished;
        [SerializeField] private EventNode[] nodeEvents;
        private DialogueManager manager;

        private void Awake()
        {
            manager = DialogueManager.Instance;
        }

        public bool Interact(GameObject target)
        {
            if (manager == null) return false;
            
            manager.StartDialogue(tree);
            return true;
        }

        private void OnDialogueNodeFinished(DialogueNode node)
        {
            var events = nodeEvents.Where(e => e.node == node).ToArray();
            foreach (var eventNode in events) eventNode.events?.Invoke();
        }

        private void OnDialogueFinished()
        {
            onDialogueFinished?.Invoke();
        }

        [Serializable]
        private struct EventNode
        {
            public DialogueNode node;
            public UnityEvent events;
        }
    }
}