using System;
using Dialogue;
using Enums;
using Systems;
using Ui;
using Ui.Notifications;
using UnityEngine;

namespace Managers
{
    public class DialogueManager : MonoBehaviour
    {
        private static DialogueManager _instance;

        [Header("Tree stuff")] [SerializeField]
        private DialogueTree currentTree;

        [SerializeField] private DialogueNode currentNode;

        [Space] [SerializeField] private DialogueController dialogueController;

        private UiManager _uiManager;

        public static DialogueManager Instance
        {
            get
            {
                _instance ??= FindObjectOfType<DialogueManager>();
                return _instance;
            }
            private set
            {
                if (_instance != null && _instance != value)
                {
                    Destroy(value);
                    return;
                }

                _instance = value;
            }
        }


        private void Awake()
        {
            Instance = this;
            _uiManager = UiManager.Instance;
        }

        public event Action OnDialogueFinished;
        public event Action<DialogueNode> OnDialogueNodeFinished;

        public void StartDialogue(DialogueTree tree)
        {
            GameManager.PlayerMovement = false;
            if (!_uiManager.ShowUiElement(WindowReference.Dialogue))
            {
                NotificationSystem.Instance.Notify(NotificationLevel.Error,
                    "Error: Dialogue UI Element was not opened.");
                return;
            }

            currentTree = tree;
            currentNode = tree.StartingNode;
            if (currentNode == null || currentNode.NextNodes.Length <= 0)
            {
                StopDialogue();
                return;
            }

            dialogueController = FindObjectOfType<DialogueController>();
            dialogueController.RenderNode(currentNode);
        }

        public void NextDialogue()
        {
            if (currentNode == null || currentNode.NextNodes.Length <= 0)
            {
                StopDialogue();
                return;
            }

            OnDialogueNodeFinished?.Invoke(currentNode);

            if (currentNode.NextNodes.Length > 1)
            {
                dialogueController.RenderOptions(currentNode);
                return;
            }

            currentNode = currentNode.NextNodes[0];

            dialogueController.RenderNode(currentNode);
        }

        public void SelectNode(int index)
        {
            index = (int) Mathf.Clamp(index, 0f, currentNode.NextNodes.Length);
            currentNode = currentNode.NextNodes[index];
            dialogueController.RenderNode(currentNode);
        }

        public void StopDialogue()
        {
            if (!_uiManager.HideUiElement(WindowReference.Dialogue)) return;

            GameManager.PlayerMovement = true;
            OnDialogueFinished?.Invoke();
        }
    }
}