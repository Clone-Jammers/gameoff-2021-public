using DG.Tweening;
using Managers;
using UnityEngine;

namespace Components.Levels
{
    public class FourthSection : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private Rigidbody exitDoor;
#pragma warning restore

        private bool isLoaded = false;

        public void LoadNextLevel()
        {
            if (!isLoaded)
            {
                isLoaded = true;
                GameManager.Instance.LoadLevel5();
            }
        }

        public void OpenExitDoor()
        {
            var openedPosition = exitDoor.transform.position - new Vector3(0, 3.5f, 0);
            exitDoor.transform.DOMove(openedPosition, 0.5f).SetEase(Ease.InSine);
        }
    }
}