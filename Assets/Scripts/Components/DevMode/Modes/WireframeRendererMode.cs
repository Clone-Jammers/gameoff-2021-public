using System;
using Components.Logger;
using UnityEngine;

namespace Components.DevMode.Modes
{
    [Serializable]
    public class WireframeRendererMode : IDevMode
    {
        private static readonly int _wireframeEnabledId = Shader.PropertyToID("_WireframeEnabled");
        private static readonly int _devModeRadiusId = Shader.PropertyToID("_DevModeRadius");

        [SerializeField] [Range(0f, 150f)] private float _devModeRadius;

        public void SetMode(bool mode)
        {
            if (mode)
            {
                GameLogger.LogInfo("Dev Mode Enabled");
            }
            else
            {
                GameLogger.LogWarning("Dev Mode Disabled");
            }

            Shader.SetGlobalInteger(_wireframeEnabledId, mode ? 1 : 0);
        }

        public void Initialize()
        {
            Shader.SetGlobalFloat(_devModeRadiusId, _devModeRadius);
        }
    }
}