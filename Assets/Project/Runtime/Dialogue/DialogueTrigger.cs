using System;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using Utils.Interfaces;

namespace Dialogue
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
            var events = nodeEvents.Where(e => e.node == node).ToArray();
            foreach (var eventNode in events) eventNode.events?.Invoke();
        }

        private void OnDialogueFinished()
        {
            manager.OnDialogueFinished -= OnDialogueFinished;
            manager.OnDialogueNodeFinished -= OnDialogueNodeFinished;

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