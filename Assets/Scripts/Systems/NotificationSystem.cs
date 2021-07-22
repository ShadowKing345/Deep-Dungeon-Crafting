using Ui.Notifications;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Systems
{
    public class NotificationSystem : MonoBehaviour
    {
        private static NotificationSystem _instance;

        public static NotificationSystem Instance
        {
            get
            {
                _instance ??= FindObjectOfType<NotificationSystem>();
                return _instance;
            }
            set
            {
                if (_instance != null && _instance != value) Destroy(value.gameObject);
                _instance = value;
            }
        }
        
        [SerializeField] private Transform notificationContainer;
        [SerializeField] private GameObject notificationPreFab;

        private void OnEnable()
        {
            Instance = this;
            GameObjectUtils.ClearChildren(notificationContainer);
            notificationPreFab.SetActive(false);
        }

        public void Notify(NotificationLevel level, string content, float timerOverride = -1f,
            bool overrideTimer = false, UnityAction callback = null)
        {
            GameObject obj = GetObj();
            if (obj.TryGetComponent(out Notification notification))
                notification.Setup(level, content, timerOverride, overrideTimer, callback);
        }

        public void Log(string content, float timerOverride = -1f, bool overrideTimer = false,
            UnityAction callback = null)
        {
            GameObject obj = GetObj();
            if (obj.TryGetComponent(out Notification notification))
                notification.Setup(NotificationLevel.Log, content, timerOverride, overrideTimer, callback);
        }

        public void Warning(string content, float timerOverride = -1f, bool overrideTimer = false,
            UnityAction callback = null)
        {
            GameObject obj = GetObj();
            if (obj.TryGetComponent(out Notification notification))
                notification.Setup(NotificationLevel.Log, content, timerOverride, overrideTimer, callback);
        }

        public void Error(string content, float timerOverride = -1f, bool overrideTimer = false,
            UnityAction callback = null)
        {
            GameObject obj = GetObj();
            if (obj.TryGetComponent(out Notification notification))
                notification.Setup(NotificationLevel.Log, content, timerOverride, overrideTimer, callback);
        }

        private GameObject GetObj()
        {
            GameObject obj = Instantiate(notificationPreFab, notificationContainer);
            obj.SetActive(true);
            return obj;
        }
    }
}