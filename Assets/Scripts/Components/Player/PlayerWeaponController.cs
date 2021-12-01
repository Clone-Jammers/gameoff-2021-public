using System;
using System.Collections;
using Components.Collectibles;
using Components.Player;
using Components.Weapons;
using Managers;
using UnityEngine;

namespace Components.Player
{
    public class PlayerWeaponController : MonoBehaviour
    {
        private bool _fireInput;
        private bool _switchInput;
        private Weapon _equippedWeapon;

        private void Awake()
        {
            // Equip if a weapon is in slot already
            _equippedWeapon = GetComponentInChildren<Weapon>();
            if (_equippedWeapon) _equippedWeapon.Equip();
        }

        public bool TryCollectWeapon(WeaponCollectible collectible)
        {
            if (_equippedWeapon) return false;
            
            collectible.Collect();
            return true;
        }

        public void EquipWeapon(Weapon weaponToEquip)
        {
            if (_equippedWeapon)
            {
                _equippedWeapon.Drop();
            }

            _equippedWeapon = weaponToEquip;
            _equippedWeapon.Equip();
        }

        public void SetInput(PlayerInput input)
        {
            HandleFireInput(input);
            HandleSwitchWeaponInput(input);
        }

        private void HandleFireInput(PlayerInput input)
        {
            if (!_equippedWeapon) return;
            
            if (_fireInput && !input.FirePressed)
            {
                _fireInput = false;
                _equippedWeapon.ReleaseTrigger();
            }
            else if (!_fireInput && input.FirePressed)
            {
                _fireInput = true;
                _equippedWeapon.PullTrigger();
            }
        }

        private void HandleSwitchWeaponInput(PlayerInput input)
        {
            if (!_switchInput && input.SwitchWeaponInput && _equippedWeapon)
            {
                _switchInput = true;
                _equippedWeapon.Drop();
                _equippedWeapon = null;

                if (SceneManager.PlayerCollectibleDetector.CurrentWeaponCollectible)
                {
                    SceneManager.PlayerCollectibleDetector.CurrentWeaponCollectible.Collect();
                }
            }
            else if (_switchInput && !input.SwitchWeaponInput)
            {
                _switchInput = false;
            }
        }
    }
}