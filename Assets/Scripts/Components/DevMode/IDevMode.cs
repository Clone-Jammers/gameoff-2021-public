using UnityEngine;

namespace Components.DevMode
{
    public interface IDevMode
    {
        void SetMode(bool isEnabled);
        void Initialize();
    }
}