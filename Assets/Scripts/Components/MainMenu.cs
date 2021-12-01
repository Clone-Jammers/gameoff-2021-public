using System;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    public class MainMenu : MonoBehaviour
    {
        #pragma warning disable 649
        [SerializeField] private Button playButton;
        [SerializeField] private Button infoButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private GameObject infoBox;
        #pragma warning restore 649

        private void Awake()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            infoButton.onClick.AddListener(OnInfoButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void OnEnable()
        {
            var color = playButton.targetGraphic.color;
            color.a = 0;

            playButton.targetGraphic.color = color;
            playButton.targetGraphic.DOFade(1, 1f)
                .SetEase(Ease.OutSine).SetUpdate(true).SetDelay(0.5f);

            color = quitButton.targetGraphic.color;
            color.a = 0;

            quitButton.targetGraphic.color = color;
            quitButton.targetGraphic.DOFade(1, 1f)
                .SetEase(Ease.OutSine).SetUpdate(true).SetDelay(0.5f);

            color = infoButton.targetGraphic.color;
            color.a = 0;

            infoButton.targetGraphic.color = color;
            infoButton.targetGraphic.DOFade(1, 1f)
                .SetEase(Ease.OutSine).SetUpdate(true).SetDelay(0.5f);
        }

        private void OnDestroy()
        {
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
            infoButton.onClick.RemoveListener(OnInfoButtonClicked);
            quitButton.onClick.RemoveListener(OnQuitButtonClicked);
        }

        private void OnInfoButtonClicked()
        {
            infoBox.SetActive(!infoBox.activeSelf);
        }

        private void OnPlayButtonClicked()
        {
            GameManager.Instance.LoadGame();
        }

        private void OnQuitButtonClicked()
        {
            GameManager.Instance.QuitGame();
        }
    }
}