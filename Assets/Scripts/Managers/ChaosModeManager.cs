using System;
using System.Collections;
using Components.ChaosMode;
using Models;
using UnityEngine;

namespace Managers
{
    public class ChaosModeManager : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.ChaosModeManager = this;
        }

        public void ToggleChaosMode(ChaosModes modes, bool state)
        {
            switch (modes)
            {
                case ChaosModes.CrosshairJuke when SceneManager.Juker:
                    SceneManager.Juker.enabled = state;
                    break;
                
                case ChaosModes.LowFriction when SceneManager.LowFriction:
                    SceneManager.LowFriction.enabled = state;
                    break;
                
                case ChaosModes.TeleportToAmmo when SceneManager.TeleportToAmmo:
                    SceneManager.TeleportToAmmo.enabled = state;
                    break;
                
                case ChaosModes.TurnYouUpsideDown when SceneManager.TurnYouUpsideDown:
                    SceneManager.TurnYouUpsideDown.enabled = state;
                    break;
            }
        }

        private void OnDestroy()
        {
            ToggleChaosMode(ChaosModes.LowFriction, false);
            ToggleChaosMode(ChaosModes.CrosshairJuke, false);
            ToggleChaosMode(ChaosModes.TeleportToAmmo, false);
            ToggleChaosMode(ChaosModes.TurnYouUpsideDown, false);
        }
    }
}