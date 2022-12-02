using System;
using Project.Runtime.Dialogue;
using UnityEngine;

namespace Project.Runtime.Managers
{
    public class DialogueManager : MonoBehaviour
    {
        private static DialogueManager _instance;

        [Header("Tree stuff")] [SerializeField]
        private DialogueTree currentTree;

        [SerializeField] private DialogueNode currentNode;

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
        }

        public void StartDialogue(DialogueTree tree)
        {
        }

        public void NextDialogue()
        {
            if (currentNode == null || currentNode.NextNodes.Length <= 0)
            {
                StopDialogue();
                return;
            }

            if (currentNode.NextNodes.Length > 1)
            {
                return;
            }

            currentNode = currentNode.NextNodes[0];
        }

        public void SelectNode(int index)
        {
            index = (int)Mathf.Clamp(index, 0f, currentNode.NextNodes.Length);
            currentNode = currentNode.NextNodes[index];
        }

        public void StopDialogue()
        {
        }
    }
}