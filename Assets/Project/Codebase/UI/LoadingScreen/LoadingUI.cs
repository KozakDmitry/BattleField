
using Assets.Project.CodeBase.Infostructure.Services;
using Assets.Project.Scripts.UI.LoadingScreen;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Project.CodeBase.UI.LoadingScreen
{
    public class LoadingUI : MonoBehaviour
    {
        /// <summary>
        /// Скрипт, который висит на самом канвасе и отвечает за его слайдер и передачу в SlideTransition
        /// </summary>
        public Slider slider;
        public TextMeshProUGUI version;

        public void Initialize(SceneTransition sceneTransition)
        {
            slider.maxValue = 1;
            slider.value = 0;
            sceneTransition.OnProgress.AddListener(UpdateSlider);
        }

        public void SetVersion(string v)
        {
            version.text = v;
        }

        private void UpdateSlider(float value)
        {
            slider.value = value;
        }
    }
}
