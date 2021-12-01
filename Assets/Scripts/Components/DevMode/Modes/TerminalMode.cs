using System;
using System.Collections.Generic;
using Components.Terminal;
using UnityEngine;

namespace Components.DevMode.Modes
{
    [Serializable]
    public class TerminalMode : IDevMode
    {
        [SerializeField] private List<TMPTerminal> _terminals;

        public void SetMode(bool isEnabled)
        {
            foreach (var terminal in _terminals)
            {
                terminal.SetActive(isEnabled);
            }
        }

        public void Initialize()
        {
            foreach (var terminal in _terminals)
            {
                terminal.SetActive(false);
            }
        }
    }
}