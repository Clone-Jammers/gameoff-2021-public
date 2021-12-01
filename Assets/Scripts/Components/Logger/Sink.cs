using System;
using System.ComponentModel;
using UnityEngine;

namespace Components.Logger
{
    public abstract class Sink : MonoBehaviour
    {
        [SerializeField] private bool _keepAtBackground;
        [SerializeField] private LogEventHub _eventHub;

        protected void Awake()
        {
            if (_keepAtBackground)
            {
                _eventHub.onLog += onEvent;
            }
        }

        protected virtual void OnEnable()
        {
            if (!_keepAtBackground)
            {
                _eventHub.onLog += onEvent;
            }
        }

        protected abstract void onEvent(string message, LogTypes types);

        protected virtual void OnDisable()
        {
            if (!_keepAtBackground)
            {
                _eventHub.onLog -= onEvent;
            }
        }

        protected void OnDestroy()
        {
            _eventHub.onLog -= onEvent;
        }
    }
}