using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Components.DevMode.Modes
{
    [Serializable]
    public class PostProcessMode : IDevMode
    {
        [SerializeField] private PostProcessVolume _postProcessVolume;
        [SerializeField] private PostProcessProfile _devModeProfile;

        private PostProcessProfile _originalProfile;

        public void SetMode(bool isEnabled)
        {
            if (isEnabled)
            {
                _originalProfile = _postProcessVolume.profile;
                _postProcessVolume.profile = _devModeProfile;
            }
            else if (_originalProfile != null)
            {
                _postProcessVolume.profile = _originalProfile;
            }
        }

        public void Initialize()
        {
        }
    }
}