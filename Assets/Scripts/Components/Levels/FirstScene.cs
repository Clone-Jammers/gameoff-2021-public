using System;
using System.Collections;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Components.Levels
{
    public class FirstScene : MonoBehaviour
    {
        #pragma warning disable 649
        [SerializeField] private Rigidbody exitDoor;
        
        [Header("Elevator")]
        [SerializeField] private float elevatorDuration;
        [SerializeField] private Rigidbody elevatorDoor;
        #pragma warning restore 649

        private void Awake()
        {
            SceneManager.FirstScene = this;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(elevatorDuration);
            var openedPosition = elevatorDoor.transform.position - new Vector3(0, 3.5f, 0);
            elevatorDoor.transform.DOMove(openedPosition, 0.5f).SetEase(Ease.InSine);
        }

        private void OnDestroy()
        {
            SceneManager.FirstScene = null;
        }

        public void OpenExitDoor()
        {
            var openedPosition = exitDoor.transform.position - new Vector3(0, 3.5f, 0);
            exitDoor.transform.DOMove(openedPosition, 0.5f).SetEase(Ease.InSine);
        }
    }
}