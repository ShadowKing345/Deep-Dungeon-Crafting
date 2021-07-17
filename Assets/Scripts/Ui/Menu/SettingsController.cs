using System;
using System.IO;
using System.Linq;
using Interfaces;
using TMPro;
using Ui.KeybindingUi;
using Ui.Tabs;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Utils;

namespace Settings
{
    public class SettingsController : MonoBehaviour, IUiWindow
    {
        [Header("Asset")]
        [SerializeField] private SettingAsset asset;

        [Space] [Header("PreFabs")]
        [SerializeField] private GameObject navigationEntryPreFab;
        [Space]
        [SerializeField] private GameObject pagePreFab;
        [Space]
        [SerializeField] private GameObject togglePreFab;
        [SerializeField] private GameObject rangePreFab;
        [SerializeField] private GameObject selectionPreFab;

        [Space] [Header("Components")] 
        [SerializeField] private GameObject navigationContent;
        [SerializeField] private GameObject bodyContent;
        [SerializeField] private CanvasGroup canvasGroup;

        [Space] [SerializeField] private TabController tabController;

        [Space] [Header("Settings Variables")] 
        [SerializeField] private AudioMixer audioMixer;
        private Resolution[] resolutions; 
        
        [Space] [Header("Key Bindings")]
        [SerializeField] private KeyBindings[] keyBindingsArray;
        [SerializeField] private GameObject keybindingEntryPreFab;

        private LTDescr _transition;

        private void OnEnable()
        {
            if(navigationEntryPreFab != null) navigationEntryPreFab.SetActive(false);
            if(togglePreFab != null) togglePreFab.SetActive(false);
            if(rangePreFab != null) rangePreFab.SetActive(false);
            if(selectionPreFab != null) selectionPreFab.SetActive(false);
            
            GameObjectUtils.ClearChildren(navigationContent.transform);
            GameObjectUtils.ClearChildren(bodyContent.transform);

            tabController ??= GetComponent<TabController>();
        }

        private void Start()
        {
            SetupSettings();
            tabController.ChangePage(tabController.TabPageDict.FirstOrDefault().Key);
        }

        public void SaveSettings()
        {
            using StreamWriter file = new StreamWriter(Path.Combine(Application.persistentDataPath, "Settings.json"));
            file.Write(JsonUtility.ToJson(asset, true));
        }

        private void SetupSettings()
        {
            foreach (SettingPage page in asset.Pages)
            {
                GameObject navEntry = Instantiate(navigationEntryPreFab, navigationContent.transform);
                navEntry.name = page.Name + "_NavEntry";
                navEntry.SetActive(true);
                Tab navTab = navEntry.GetComponent<Tab>();
                navTab.ToolTipText = page.Name;
                navTab.Sprite = page.Image;
                
                GameObject pageObj = Instantiate(pagePreFab, bodyContent.transform);
                pageObj.name = page.Name + "_Page";
                TabPage tabPage = pageObj.GetComponent<TabPage>();

                tabController.AddPage(navTab, pageObj);

                if (page.Name == "keybindings")
                {
                    KeyBindingSetup(pageObj);
                    continue;
                }

                foreach (SettingEntry entry in page.Settings)
                {
                    GameObject entryObj = Instantiate(entry.Type switch
                    {
                        SettingEntryValueType.Toggle => togglePreFab,
                        SettingEntryValueType.Range => rangePreFab,
                        SettingEntryValueType.Selection => selectionPreFab,
                        _ => throw new ArgumentOutOfRangeException()
                    }, tabPage.content);
                    entryObj.SetActive(true);

                    entryObj.GetComponentInChildren<TextMeshProUGUI>().text = entry.Name;

                    switch (entry.Name)
                    {
                        case "resolution":
                            ResolutionSetup(entry, entryObj);
                            break;
                        default:
                            switch (entry.Type)
                            {
                                case SettingEntryValueType.Toggle:
                                    if (entryObj.TryGetComponent(out Toggle toggle)) toggle.isOn = entry.ToggleValue;
                                    break;
                                case SettingEntryValueType.Range:
                                    if (entryObj.TryGetComponent(out Slider slider))
                                    {
                                        slider.minValue = entry.RangeRanges.Min;
                                        slider.maxValue = entry.RangeRanges.Max;
                                        slider.value = entry.RangeValue;
                                    }
                            
                                    break;
                                case SettingEntryValueType.Selection:
                                    if (entryObj.TryGetComponent(out TMP_Dropdown dropdown))
                                    {
                                        dropdown.options = entry.Selection.Select(s => new TMP_Dropdown.OptionData(s))
                                            .ToList();
                                        dropdown.value = entry.SelectionValue;
                                    }
                            
                                    ;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                    }
                }
            }
        }

        private void ResolutionSetup(SettingEntry entry, GameObject entryObj)
        {
            resolutions = Screen.resolutions;
            TMP_Dropdown resolutionDropDown = entryObj.GetComponentInChildren<TMP_Dropdown>();
            
            resolutionDropDown.options.AddRange(resolutions.Select(res =>
                new TMP_Dropdown.OptionData($"{res.width}x{res.height}")));

            for (int i = 0; i < resolutions.Length; i++)
                if (resolutions[i].Equals(Screen.currentResolution))
                    resolutionDropDown.value = i;
        }

        private void KeyBindingSetup(GameObject page)
        {
            page.SetActive(true);
            
            KeyBindingController keyBindingController = page.AddComponent<KeyBindingController>();
            keyBindingController.content = page.GetComponent<TabPage>().content;
            keyBindingController.entryCollection = keyBindingsArray;
            keyBindingController.entryPreFab = keybindingEntryPreFab;
            keyBindingController.GenerateKeyBindings();
            
            page.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _transition = LeanTween.alphaCanvas(canvasGroup, 1, 0.1f).setIgnoreTimeScale(true);
        }

        public void Hide()
        {
            if(_transition != null) LeanTween.cancel(_transition.uniqueId);
            LeanTween.alphaCanvas(canvasGroup, 0, 0.1f).setOnComplete(_ => gameObject.SetActive(false)).setIgnoreTimeScale(true);
        }
    }
}