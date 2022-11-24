using UnityEngine;

namespace Managers
{
    public class LoadingScreenManager : MonoBehaviour
    {
        private static LoadingScreenManager _instance;

        [SerializeField] private GameObject loadingScreen;

        private static LoadingScreenManager Instance
        {
            get
            {
                _instance ??= FindObjectOfType<LoadingScreenManager>();
                return _instance;
            }
            set
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

        // private LTDescr transitions;

        public static void HideScreen()
        {
            var instance = Instance;
            var loadingScreen = instance.loadingScreen;
            var cg = instance.loadingScreen.GetComponent<CanvasGroup>();

            loadingScreen.SetActive(true);
            // if(instance.transitions != null) LeanTween.cancel(instance.transitions.uniqueId);

            // instance.transitions =
            // LeanTween.alphaCanvas(cg, 1f, 0.5f);
        }

        public static void ShowScreen()
        {
            var instance = Instance;
            var loadingScreen = instance.loadingScreen;
            var cg = instance.loadingScreen.GetComponent<CanvasGroup>();

            // if(instance.transitions != null) LeanTween.cancel(instance.transitions.uniqueId);

            // instance.transitions =
            // LeanTween.alphaCanvas(cg, 0f, 0.5f).setOnComplete(_ => loadingScreen.SetActive(false));
        }
    }
}