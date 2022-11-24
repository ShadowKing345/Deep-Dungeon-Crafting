using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Runtime.Ui.Notifications
{
    public class Notification : Button
    {
        [Header("Internal Components")] [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField] private Image targetImage;
        [SerializeField] private CanvasGroup cg;

        [Space] [Header("Config")] [SerializeField]
        private Color logColor;

        [SerializeField] private Color warnColor;
        [SerializeField] private Color errorColor;

        [Space] [SerializeField] private NotificationLevel level;

        private UnityAction callback;
        // private LTDescr leanTween;

        public void Setup(NotificationLevel level, string content, float timerOverride = -1f,
            bool overrideTimer = false, UnityAction callback = null)
        {
            this.level = level;
            text.text = content;

            targetImage.color = level switch
            {
                NotificationLevel.Log => logColor,
                NotificationLevel.Warning => warnColor,
                NotificationLevel.Error => errorColor,
                _ => logColor
            };

            // LeanTween.alphaCanvas(cg, 1, 0.1f);
            // leanTween = LeanTween.delayedCall(overrideTimer ? timerOverride : level.GetStandardDuration(),
            // () => LeanTween.alphaCanvas(cg, 0, 0.1f).setOnComplete(_ => Destroy(gameObject)));

            this.callback = callback;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    callback?.Invoke();
                    break;
                case PointerEventData.InputButton.Right:
                    // if (leanTween != null)
                    // {
                    //     leanTween.callOnCompletes();
                    //     LeanTween.cancel(leanTween.uniqueId);
                    // }
                    // else
                    Destroy(gameObject);
                    break;
                case PointerEventData.InputButton.Middle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}