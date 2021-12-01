using System;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class TriggerVolume : MonoBehaviour
    {
        public UnityEvent triggerEnter;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                triggerEnter.Invoke();
            }
        }
    }
}