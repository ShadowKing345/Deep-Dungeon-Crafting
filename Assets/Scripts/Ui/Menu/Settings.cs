using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Ui.Menu
{
    public class Settings : MonoBehaviour, IUiElement
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Dropdown resolutionDropDown;
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
        }

        public void SetQuality(int index)
        {
            
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

        public void Show()
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public static class ResolutionExtension
    {
        public static bool Equals(this Resolution res, Resolution comparedTo) => res.width == comparedTo.width && res.height == comparedTo.height;
    }
}