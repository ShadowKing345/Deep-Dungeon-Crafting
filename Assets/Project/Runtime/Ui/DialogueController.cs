using System.Collections;
using Project.Runtime.Dialogue;
using Project.Runtime.Enums;
using Project.Runtime.Managers;
using Project.Runtime.Utils;
using Project.Runtime.Utils.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Runtime.Ui
{
    public class DialogueController : MonoBehaviour, IUiWindow
    {
        [Space] [Header("Components")] [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField] private TextMeshProUGUI youDialogueName;
        [SerializeField] private Image youImage;
        [SerializeField] private TextMeshProUGUI themDialogueName;
        [SerializeField] private Image themImage;
        [SerializeField] private TextMeshProUGUI bodyText;
        [SerializeField] private GameObject continueButton;

        [Space] [SerializeField] private GameObject optionsObj;

        [SerializeField] private Transform content;
        [SerializeField] private GameObject optionPreFab;
        private UiManager _uiManager;

        private void Awake()
        {
            _uiManager = UiManager.Instance;
            _uiManager.RegisterWindow(WindowReference.Dialogue, gameObject);

            gameObject.SetActive(false);

            continueButton.GetComponent<Button>().onClick.AddListener(DialogueManager.Instance.NextDialogue);
        }

        private void OnDestroy()
        {
            _uiManager.UnregisterWindow(WindowReference.Dialogue, gameObject);
        }

        public void Show()
        {
        }
        // LeanTween.alphaCanvas(canvasGroup, 1f, 0.3f).setOnComplete(_ => canvasGroup.interactable = canvasGroup.blocksRaycasts = true);

        public void Hide()
        {
            canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }

        public void RenderNode(DialogueNode node)
        {
            optionsObj.SetActive(false);
            bodyText.enabled = true;
            continueButton.SetActive(true);

            if (!string.IsNullOrEmpty(node.CharacterName))
                themDialogueName.text = node.CharacterName;

            StopAllCoroutines();
            StartCoroutine(ChangeColorForYouAndThem(!node.IsYou, node.IsYou));
            bodyText.text = node.Body;
        }

        private IEnumerator ChangeColorForYouAndThem(bool youGray, bool themGray)
        {
            var you = youGray ? Color.gray : Color.white;
            var them = themGray ? Color.gray : Color.white;

            while (youImage.color != you || themImage.color != them)
            {
                youImage.color = youDialogueName.color = Color.Lerp(youImage.color, you, 0.1f);
                themImage.color = themDialogueName.color = Color.Lerp(themImage.color, them, 0.1f);
                yield return new WaitForEndOfFrame();
            }
        }

        public void RenderOptions(DialogueNode node)
        {
            bodyText.enabled = false;
            optionsObj.SetActive(true);
            continueButton.SetActive(false);
            GameObjectUtils.ClearChildren(content);

            youImage.color = youDialogueName.color =
                themImage.color = themDialogueName.color = Color.gray;

            for (var i = 0; i < node.NextNodes.Length; i++)
            {
                var index = i;
                var button = Instantiate(optionPreFab, content);
                if (button.TryGetComponent(out Button b))
                    b.onClick.AddListener(() => DialogueManager.Instance.SelectNode(index));
                var text = button.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null) text.text = node.NextNodes[i].OptionName;
            }

            content.GetComponentInChildren<Selectable>().Select();
        }
    }
}