using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Ui.Menu
{
    public class Settings : MonoBehaviour, IUiElement
    {
        private LTDescr _transition;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Dropdown resolutionDropDown;
        [SerializeField] private TMP_Dropdown qualityDropDown;
        [SerializeField] private Toggle fullScreenToggle;
        
        [SerializeField] private AudioMixer audioMixer;
        private Resolution[] resolutions;

        private void Start()
        {
            canvasGroup ??= GetComponent<CanvasGroup>();

            resolutions = Screen.resolutions;
            resolutionDropDown.options.AddRange(resolutions.Select(res =>
                new TMP_Dropdown.OptionData($"{res.width}x{res.height}")));

            for (int i = 0; i < resolutions.Length; i++)
                if (resolutions[i].Equals(Screen.currentResolution))
                    resolutionDropDown.value = i;

            fullScreenToggle.isOn = Screen.fullScreen;

            qualityDropDown.value = QualitySettings.GetQualityLevel();
        }

        public void SetQuality(int index)
        {
            QualitySettings.SetQualityLevel(index);
        }
        
        public void SetResolution(int index)
        {
            Resolution resolution = resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
        
        public void SetFullScreen(bool value)
        {
            Screen.fullScreen = value;
        }
        
        public void SetVolume(float level)
        {
            if(audioMixer == null) return;
            audioMixer.SetFloat("Volume", level);
        }

        public void Back()
        {
            Hide();
        }

        public void Show() =>
            _transition = LeanTween.alphaCanvas(canvasGroup, 1, 0.3f).setOnComplete(_ =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }).setIgnoreTimeScale(true);

        public void Hide()
        {
            if(_transition != null) LeanTween.cancel(_transition.uniqueId);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            _transition = LeanTween.alphaCanvas(canvasGroup, 0, 0.3f).setIgnoreTimeScale(true);
        }
    }

    public static class ResolutionExtension
    {
        public static bool Equals(this Resolution res, Resolution comparedTo) => res.width == comparedTo.width && res.height == comparedTo.height;
    }
}