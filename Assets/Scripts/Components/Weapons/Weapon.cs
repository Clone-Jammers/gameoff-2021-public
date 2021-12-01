using System;
using Components.Collectibles;
using Components.Logger;
using KinematicCharacterController;
using Managers;
using UnityEngine;

namespace Components.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        #pragma warning disable 649
        [SerializeField] private Collectible collectible;
        #pragma warning restore 649
        
        public abstract void PullTrigger();
        public abstract void ReleaseTrigger();
        public abstract void Fire();
        public abstract void Equip();

        public void Drop()
        {
            GameLogger.LogDebug($"Weapon Dropped.");
            Destroy(gameObject);
            Instantiate(collectible, transform.position, transform.rotation)
                .Launch(transform.forward * (SceneManager.PlayerCharacterController.GroundSpeed + 10));
        }
    }
}