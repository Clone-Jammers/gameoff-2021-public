using Components.Weapons;
using Managers;
using UnityEngine;

namespace Components.Collectibles
{
    public class WeaponCollectible : Collectible
    {
        #pragma warning disable 649
        [SerializeField] private Weapon weapon;
        #pragma warning restore 649
        
        protected override void OnCollect()
        {
            var instance = Instantiate(weapon, SceneManager.PlayerWeaponController.transform);
            SceneManager.PlayerWeaponController.EquipWeapon(instance);
            Destroy(gameObject);
        }
    }
}