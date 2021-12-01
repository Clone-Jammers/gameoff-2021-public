using System;
using System.Collections;
using System.Collections.Generic;
using Components;
using Components.Logger;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private const int MenuScene = 1;
        private const int Scene1 = 2;
        private const int Scene2 = 3;
        private const int Scene3 = 4;
        private const int Scene4 = 5;
        private const int Scene5 = 6;
        private const int Scene6 = 7;

        private static GameManager _instance;
        public static GameManager Instance => _instance ? _instance : _instance = FindObjectOfType<GameManager>();

        public bool Playing { get; private set; }
        public bool Paused { get; private set; }
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            UnitySceneManager.LoadSceneAsync(MenuScene, LoadSceneMode.Additive);
            EventManager.PlayerDied += OnPlayerDied;
        }

        private void OnDestroy()
        {
            EventManager.PlayerDied -= OnPlayerDied;
        }

        public void TogglePause(bool paused)
        {
            if (!Playing) return;
            
            Paused = paused;
            
            Time.timeScale = paused ? 0 : 1;
            Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = paused;

            if (paused)
            {
                SceneManager.PauseMenu.Open();
            }
        }

        public void LoadGame()
        {
            Playing = true;
            
            TogglePause(false);
            FadeTransition.Instance.FadeIn(() => StartCoroutine(Routine()));
            
            
            IEnumerator Routine()
            {
                GameLogger.LogDebug("Unloading: Menu");
                var unloadOp = UnitySceneManager.UnloadSceneAsync(MenuScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                unloadOp.allowSceneActivation = true;
                
                GameLogger.LogDebug("Loading: Section-A");
                var loadOp1 = UnitySceneManager.LoadSceneAsync(Scene1, LoadSceneMode.Additive);
                loadOp1.allowSceneActivation = true;
                
                GameLogger.LogDebug("Loading: Section-B");
                var loadOp2 = UnitySceneManager.LoadSceneAsync(Scene2, LoadSceneMode.Additive);
                loadOp2.allowSceneActivation = true;

                yield return unloadOp;
                yield return loadOp1;
                yield return loadOp2;
                GameLogger.LogDebug("Unloaded: Menu");
                GameLogger.LogDebug("Loaded: Section-A");
                GameLogger.LogDebug("Loaded: Section-B");

                GameLogger.LogInfo("Game starting...");
                UnitySceneManager.SetActiveScene(UnitySceneManager.GetSceneByBuildIndex(Scene1));
                FadeTransition.Instance.FadeOut();
            }
        }

        public void LoadLevel3()
        {
            GameLogger.LogDebug("Loading: Section-C");
            var op = UnitySceneManager.LoadSceneAsync(Scene3, LoadSceneMode.Additive);
            op.allowSceneActivation = true;
        }

        public void LoadLevel4()
        {
            GameLogger.LogDebug("Loading: Section-D");
            var op = UnitySceneManager.LoadSceneAsync(Scene4, LoadSceneMode.Additive);
            op.allowSceneActivation = true;
        }

        public void LoadLevel5()
        {
            GameLogger.LogDebug("Loading: Section-E");
            var op = UnitySceneManager.LoadSceneAsync(Scene5, LoadSceneMode.Additive);
            op.allowSceneActivation = true;
        }

        public void LoadLevel6()
        {
            GameLogger.LogDebug("Loading: Section-F");
            var op = UnitySceneManager.LoadSceneAsync(Scene6, LoadSceneMode.Additive);
            op.allowSceneActivation = true;
        }

        public void LoadMenu()
        {
            Playing = false;
            
            FadeTransition.Instance.FadeIn(() =>
            {
                StartCoroutine(Routine());
                SceneManager.DeathMenu.Close();
                SceneManager.PauseMenu.CloseNow();
            });
            
            

            IEnumerator Routine()
            {
                var list = new List<AsyncOperation>();
                
                GameLogger.LogDebug("Unlading: Section-A");
                GameLogger.LogDebug("Unlading: Section-B");
                list.Add(UnitySceneManager.UnloadSceneAsync(Scene1, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects));
                list.Add(UnitySceneManager.UnloadSceneAsync(Scene2, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects));

                if (UnitySceneManager.GetSceneByBuildIndex(Scene3).isLoaded)
                {
                    GameLogger.LogDebug("Unlading: Section-C");
                    list.Add(UnitySceneManager.UnloadSceneAsync(Scene3, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects));
                }

                if (UnitySceneManager.GetSceneByBuildIndex(Scene4).isLoaded)
                {
                    GameLogger.LogDebug("Unlading: Section-D");
                    list.Add(UnitySceneManager.UnloadSceneAsync(Scene4, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects));
                }

                if (UnitySceneManager.GetSceneByBuildIndex(Scene5).isLoaded)
                {
                    GameLogger.LogDebug("Unlading: Section-E");
                    list.Add(UnitySceneManager.UnloadSceneAsync(Scene5, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects));
                }

                // if (UnitySceneManager.GetSceneByBuildIndex(Scene6).isLoaded)
                // {
                //     GameLogger.LogDebug("Unlading: Section-F");
                //     UnitySceneManager.UnloadSceneAsync(Scene6, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                // }

                foreach (var entry in list)
                {
                    yield return entry;
                }
            
                GameLogger.LogDebug("Loading: Menu");
                UnitySceneManager.LoadScene(MenuScene, LoadSceneMode.Additive);
                FadeTransition.Instance.FadeOut();
                
                GameLogger.LogInfo("Menu is ready...");

                yield return null;
            }
        }

        public void QuitGame()
        {
            FadeTransition.Instance.FadeIn(() => StartCoroutine(Routine()));
            

            IEnumerator Routine()
            {
                yield return null;
                Application.Quit();
            }
        }

        private void OnPlayerDied()
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            SceneManager.DeathMenu.Open();
            Playing = false;
        }
    }
}