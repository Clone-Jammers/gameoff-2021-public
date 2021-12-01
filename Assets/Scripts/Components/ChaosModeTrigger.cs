using System;
using Managers;
using Models;
using UnityEngine;

namespace Components
{
    public class ChaosModeTrigger : MonoBehaviour
    {
        #pragma warning disable 649
        [SerializeField] private bool chaosInside;
        [SerializeField] private ChaosModes chaosMode;
        #pragma warning restore 649

        private void OnDisable()
        {
            SceneManager.ChaosModeManager.ToggleChaosMode(chaosMode, false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.ChaosModeManager.ToggleChaosMode(chaosMode, chaosInside);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.ChaosModeManager.ToggleChaosMode(chaosMode, !chaosInside);
            }
        }
    }
}