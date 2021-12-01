using System;
using System.Collections.Generic;
using Components.Player;
using UnityEngine;

namespace Components.DevMode
{
    public class DevModeController : MonoBehaviour
    {
        [SerializeReference] private List<IDevMode> _modes;
        private static readonly int _wireframeEnabledId = Shader.PropertyToID("_WireframeEnabled");
        private static readonly int _devModeRadiusId = Shader.PropertyToID("_DevModeRadius");
        
        private bool activateDevMode;

        private void Start()
        {
            foreach (var mode in _modes)
            {
                mode.Initialize();
            }
        }

        public void SetInput(PlayerInput input)
        {
            if (activateDevMode == input.DevModePressed) return;
            
            SetDevMode(input.DevModePressed);
            activateDevMode = input.DevModePressed;
        }

        private void SetDevMode(bool value)
        {
            foreach (var mode in _modes)
            {
                mode.SetMode(value);
            }
        }
    }
}