using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    public class PauseMenu : MonoBehaviour
    {
        #pragma warning disable 649
        [SerializeField] private Graphic[] fadeGraphics;
        [SerializeField] private Button toMainMenuButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button quitButton;
        #pragma warning disable 649

        private bool _animating;
        private bool _open;

        private void Awake()
        {
            SceneManager.PauseMenu = this;
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
            _animating = true;

            for (var index = 0; index < fadeGraphics.Length; index++)
            {
                var graphic = fadeGraphics[index];
                
                if (index == fadeGraphics.Length - 1)
                {
                    graphic.DOFade(0, 1f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                        Unbind();
                        _animating = false;
                    });
                }
                else
                {
                    graphic.DOFade(0, 1f).SetEase(Ease.Linear).SetUpdate(true);
                }
            }
        }

        public void CloseNow()
        {
            _animating = false;
            _open = false;
            gameObject.SetActive(false);
        }

        private void Bind()
        {
            quitButton.onClick.AddListener(OnQuitClicked);
            resumeButton.onClick.AddListener(OnResumeClicked);
            toMainMenuButton.onClick.AddListener(OnToMainMenuClicked);
        }

        private void Unbind()
        {
            toMainMenuButton.onClick.RemoveListener(OnToMainMenuClicked);
            resumeButton.onClick.RemoveListener(OnResumeClicked);
            quitButton.onClick.RemoveListener(OnQuitClicked);
        }

        private void OnToMainMenuClicked()
        {
            GameManager.Instance.LoadMenu();
        }

        private void OnQuitClicked()
        {
            GameManager.Instance.QuitGame();
        }

        private void OnResumeClicked()
        {
            GameManager.Instance.TogglePause(false);
            Close();
        }
    }
}