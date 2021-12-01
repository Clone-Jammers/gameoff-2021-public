using System;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    public class DeathMenu : MonoBehaviour
    {
        #pragma warning disable 649
        [SerializeField] private Graphic[] fadeGraphics;
        [SerializeField] private Button toMainMenuButton;
        #pragma warning disable 649

        private bool _animating;
        private bool _open;

        private void Awake()
        {
            SceneManager.DeathMenu = this;
            gameObject.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }

        private void OnDisable()
        {
            Unbind();
        }

        public void Open()
        {
            if (_animating || _open) return;

            _animating = true;
            gameObject.SetActive(true);
            for (var index = 0; index < fadeGraphics.Length; index++)
            {
                var graphic = fadeGraphics[index];
                var c = graphic.color;
                c.a = 0;
                graphic.color = c;

                if (index == fadeGraphics.Length - 1)
                {
                    graphic.DOFade(1, 1f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                    {
                        Bind();
                        _animating = false;
                        _open = true;
                    });
                }
                else
                {
                    graphic.DOFade(1, 1f).SetEase(Ease.Linear).SetUpdate(true);
                }
            }
        }

        public void Close()
        {
            if (_animating || !_open) return;

            _open = false;
            _animating = false;
            
            gameObject.SetActive(false);
        }

        private void Bind()
        {
            toMainMenuButton.onClick.AddListener(OnToMainMenuClick);
        }

        private void Unbind()
        {
            toMainMenuButton.onClick.RemoveListener(OnToMainMenuClick);
        }

        private void OnToMainMenuClick()
        {
            GameManager.Instance.LoadMenu();
        }
    }
}