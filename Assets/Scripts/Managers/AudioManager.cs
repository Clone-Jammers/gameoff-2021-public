using System;
using Components.Weapons;
using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{    
    public delegate void ObjectSfxDelegate(string sfx, Transform source);

    public class AudioManager : MonoBehaviour
    {
        private void Awake()
        {
            
        }

        private void OnDestroy()
        {
            
        }

        private void OnShotgunFired(string sfx, Transform _)
        {
            
        }
    }
}