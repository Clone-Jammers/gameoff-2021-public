using System.Collections.Generic;
using Components.Collectibles;
using Managers;
using UnityEngine;

namespace Components.Player
{
    public class PlayerCollectibleDetector : MonoBehaviour
    {
        private WeaponCollectible _currentWeaponCollectible;

        private readonly List<Collectible> _weapons = new List<Collectible>();
        
        public WeaponCollectible CurrentWeaponCollectible => _currentWeaponCollectible;

        private void Awake()
        {
            SceneManager.PlayerCollectibleDetector = this;
            
            Collectible.Collected += OnCollected;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Collectible"))
            {
                if (other.TryGetComponent(out WeaponCollectible weaponCollectible))
                {
                    if (!SceneManager.PlayerWeaponController.TryCollectWeapon(weaponCollectible))
                    {
                        _weapons.Add(weaponCollectible);
                        FindClosestWeapon();
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Collectible"))
            {
                if (other.TryGetComponent(out WeaponCollectible weaponCollectible))
                {
                    _weapons.Remove(weaponCollectible);
                    FindClosestWeapon();
                }
            }
        }

        private void OnCollected(Collectible collectible)
        {
            switch (collectible)
            {
                case WeaponCollectible weaponCollectible:
                    _weapons.Remove(weaponCollectible);
                    FindClosestWeapon();
                    break;
            }
        }

        private void FindClosestWeapon()
        {
            Collectible current = null;
            var distance = float.MaxValue;

            var pos = transform.position;
            pos.y = 0;

            foreach (var collectible in _weapons)
            {
                var curPos = collectible.transform.position;
                curPos.y = 0;
                
                var newDistance = Vector3.Distance(curPos, pos);

                if (newDistance < distance)
                {
                    current = collectible;
                    distance = newDistance;
                }
            }

            _currentWeaponCollectible = current as WeaponCollectible;
        }
    }
}