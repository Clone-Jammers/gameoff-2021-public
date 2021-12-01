using System;
using Components.DevMode;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Components.Player
{
    public struct PlayerInput
    {
        public Vector2 LookInput;
        public Vector2 MovementInput;
        public bool JumpPressed;
        public bool FirePressed;
        public bool SwitchWeaponInput;
        public bool InteractInput;
        public bool DevModePressed;
    }

    public class PlayerController : MonoBehaviour
    {
        private PlayerCharacterController _characterController;
        private DevModeController _devModeController;
        private PlayerWeaponController _weaponController;
        private GameInputActions _inputActions;
        private PlayerInput _input;

        private Vector2? _mousePosition;

        private void Awake()
        {
            _inputActions = new GameInputActions();
            _weaponController = GetComponentInChildren<PlayerWeaponController>();
            _characterController = GetComponent<PlayerCharacterController>();
            _devModeController = GetComponent<DevModeController>();

            SceneManager.PlayerController = this;
            SceneManager.PlayerWeaponController = _weaponController;
            SceneManager.PlayerCharacterController = _characterController;

            EventManager.PlayerDied += OnPlayerDied;
        }

        private void OnEnable()
        {
            _inputActions.Player.Pause.performed += OnPausePerformed;
            
            _inputActions.Enable();
        }

        private void Update()
        {
            // Poll input
            _input.LookInput = _inputActions.Player.Look.ReadValue<Vector2>();
            _input.MovementInput = _inputActions.Player.Move.ReadValue<Vector2>();
            _input.FirePressed = _inputActions.Player.Fire.ReadValue<float>() > 0.1f;
            _input.JumpPressed = _inputActions.Player.Jump.ReadValue<float>() > 0.1f;
            _input.InteractInput = _inputActions.Player.Interact.ReadValue<float>() > 0.1f;
            _input.SwitchWeaponInput = _inputActions.Player.SwitchWeapons.ReadValue<float>() > 0.1f;
            _input.DevModePressed = _inputActions.Player.DevMode.ReadValue<float>() > 0.1f;

            // Apply sensitivity to look input
            _input.LookInput.x *= SettingsManager.Settings.horizontalSensitivity;
            _input.LookInput.y *= SettingsManager.Settings.verticalSensitivity;

            // Dispatch
            _weaponController.SetInput(_input);
            _characterController.SetInput(_input);
            _devModeController.SetInput(_input);
        }

        private void OnDisable()
        {
            _inputActions.Player.Pause.performed -= OnPausePerformed;
            _inputActions.Disable();
        }

        private void OnDestroy()
        {
            EventManager.PlayerDied -= OnPlayerDied;
        }

        private void OnPlayerDied()
        {
            enabled = false;
        }

        private void OnPausePerformed(InputAction.CallbackContext _)
        {
            if (GameManager.Instance.Paused)    GameManager.Instance.TogglePause(false);
            else                                GameManager.Instance.TogglePause(true);
        }
    }
}