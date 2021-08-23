using System;
using System.Linq;
using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour, IInteractable
    {
        private DialogueManager manager;
        
        [Serializable]
        private struct EventNode
        {
            public DialogueNode node;
            public UnityEvent events;
        }

        
        [SerializeField] private DialogueTree tree;

        [SerializeField] private UnityEvent onDialogueFinished;
        [SerializeField] private EventNode[] nodeEvents;

        private void Awake() => manager = DialogueManager.Instance;
        private void OnDestroy()
        {
            manager.OnDialogueFinished -= OnDialogueFinished;
            manager.OnDialogueNodeFinished -= OnDialogueNodeFinished;
        }

        public bool Interact(GameObject target)
        {
            if (manager == null) return false;

            manager.OnDialogueFinished += OnDialogueFinished;
            manager.OnDialogueNodeFinished += OnDialogueNodeFinished;
            
            manager.StartDialogue(tree);
            return true;
        }

        private void OnDialogueNodeFinished(DialogueNode node)
        {
            EventNode[] events = nodeEvents.Where(e => e.node == node).ToArray();
            foreach (EventNode eventNode in events) eventNode.events?.Invoke();
        }
        
        private void OnDialogueFinished()
        {
            manager.OnDialogueFinished -= OnDialogueFinished;
            manager.OnDialogueNodeFinished -= OnDialogueNodeFinished;
            
            onDialogueFinished?.Invoke();
        }
    }
}