using System;
using System.Linq;
using Components.Logger;
using Managers;
using UnityEngine;

namespace Components.ChaosMode
{
    public class TurnYouUpsideDown : MonoBehaviour
    {
        #pragma warning disable 649
        [SerializeField] private Transform _eye;
        [SerializeField] private Transform _weaponCam;
        #pragma warning restore 649

        private void Awake()
        {
            SceneManager.TurnYouUpsideDown = this;
            enabled = false;
        }

        private void OnEnable()
        {
            GameLogger.LogError("CAMERA : Camera local up direction is not valid.");
            _eye.localRotation = Quaternion.Euler(0, 0, 180);
            _weaponCam.localRotation = Quaternion.Euler(0, 0, 180);
        }

        private void OnDisable()
        {
            GameLogger.LogInfo("CAMERA : Camera local up direction set to normal");
            _eye.localRotation = Quaternion.identity;
            _weaponCam.localRotation = Quaternion.identity;
        }
    }
}