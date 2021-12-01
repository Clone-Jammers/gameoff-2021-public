using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    public class FadeTransition : MonoBehaviour
    {
        private static FadeTransition _instance;
        public static FadeTransition Instance => _instance ? _instance : _instance = FindObjectOfType<FadeTransition>();
        
        #pragma warning disable 649
        [SerializeField] private Image fadeImage;
        #pragma warning restore 649

        private void Awake()
        {
            var instance = Instance;
            DontDestroyOnLoad(gameObject);
            gameObject.SetActive(false);
        }

        public void FadeIn(Action onComplete = null)
        {
            gameObject.SetActive(true);
            fadeImage.DOFade(1, 1f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }

        public void FadeOut(Action onComplete = null)
        {
            fadeImage.DOFade(0, 1f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
            {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }
    }
}