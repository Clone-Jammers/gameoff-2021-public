using System;
using Components.Logger;
using Components.Player;
using Managers;
using UnityEngine;

namespace Components.ChaosMode
{
    public class LowFriction : MonoBehaviour
    {
        [SerializeField] private PlayerCharacterController _playerCharacterController;
        [Range(0, 30)] [SerializeField] private float _frictionRate;

        private float _previousValue;

        private void Awake()
        {
            SceneManager.LowFriction = this;
            _previousValue = _playerCharacterController.FrictionRate;
            enabled = false;
        }

        private void OnEnable()
        {
            GameLogger.LogError("PHYSIC ENGINE: Friction value is missing!");
            _playerCharacterController.FrictionRate = _frictionRate;
        }

        private void OnDisable()
        {
            GameLogger.LogInfo("PHYSIC ENGINE: Friction value set to normal.");
            _playerCharacterController.FrictionRate = _previousValue;
        }
    }
}