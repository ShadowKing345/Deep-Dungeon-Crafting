using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Utils.Interfaces;

namespace Ui.Menu
{
    public class Settings : MonoBehaviour, IUiWindow
    {
        private static Resolution[] _resolutions;
        // private LTDescr _transition;

        [Header("Components")] [SerializeField]
        private CanvasGroup canvasGroup;

        [Space] [Header("Controls")] [SerializeField]
        private TMP_Dropdown resolutionDropDown;

        [SerializeField] private TMP_Dropdown qualityDropDown;
        [SerializeField] private Toggle fullScreenToggle;
        [SerializeField] private Slider volumeSlider;

        [Space] [Header("External Assets")] [SerializeField]
        private AudioMixer audioMixer;

        private void OnEnable()
        {
            canvasGroup ??= GetComponent<CanvasGroup>();

            _resolutions = Screen.resolutions;
            if (resolutionDropDown != null)
            {
                resolutionDropDown.onValueChanged.AddListener(SetResolution);
                resolutionDropDown.options.AddRange(_resolutions.Select(res =>
                    new TMP_Dropdown.OptionData($"{res.width}x{res.height}")));

                for (var i = 0; i < _resolutions.Length; i++)
                    if (_resolutions[i].Equals(Screen.currentResolution))
                        resolutionDropDown.value = i;
            }

            if (fullScreenToggle != null)
            {
                fullScreenToggle.onValueChanged.AddListener(SetFullScreen);
                fullScreenToggle.isOn = Screen.fullScreen;
            }

            if (qualityDropDown != null)
            {
                qualityDropDown.onValueChanged.AddListener(SetQuality);
                qualityDropDown.value = QualitySettings.GetQualityLevel();
            }

            if (volumeSlider != null)
            {
                volumeSlider.onValueChanged.AddListener(SetVolume);
                volumeSlider.minValue = -80;
                volumeSlider.maxValue = 0;
                if (audioMixer.GetFloat("Volume", out var volumeLevel))
                    volumeSlider.value = volumeLevel;
            }
        }

        public void Show()
        {
            throw new NotImplementedException();
        }

        public void Hide()
        {
            throw new NotImplementedException();
        }

        private static void SetQuality(int index)
        {
            QualitySettings.SetQualityLevel(index);
        }

        private static void SetResolution(int index)
        {
            var resolution = _resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        private static void SetFullScreen(bool value)
        {
            Screen.fullScreen = value;
        }

        private void SetVolume(float level)
        {
            if (audioMixer == null) return;
            audioMixer.SetFloat("Volume", level);
        }

        public void Back()
        {
            Hide();
        }
    }
}