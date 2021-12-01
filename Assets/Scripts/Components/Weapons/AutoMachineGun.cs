using System;
using System.Collections;
using Components.Logger;
using DG.Tweening;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities;

namespace Components.Weapons
{
    public class AutoMachineGun : Weapon
    {
        private static readonly int FireHash = Animator.StringToHash("fire");
        
        #pragma warning disable 649
        [SerializeField] private Animator animator;
        [SerializeField] private Transform muzzle;
        [SerializeField] private GameObject weaponMesh;
        
        [Title("VFX")]
        [SerializeField] private GameObject bulletDecal;
        [SerializeField] private GameObject hitParticle;
        [SerializeField] private GameObject muzzleParticle;

        [Title("Parameters")] 
        [SerializeField] private int damage;
        [SerializeField] private float effectiveRange;
        [SerializeField] private float maxSpreadDuration;
        [SerializeField] private AnimationCurve spreadCurve;
        #pragma warning restore 649

        private bool _equipped;
        private bool _firing;
        private bool _requestFire;
        private bool _waitToFire;
        
        private float _spreadPower;
        private float _currentSpread;
        private float _timeSinceFireRequested;

        private void Awake()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            _equipped = false;
            animator.enabled = true;
            weaponMesh.SetActive(false);
        }

        private void Update()
        {
            if (!_equipped) return;
            
            if (_firing)
            {
                _spreadPower = Mathf.Clamp01(_spreadPower + Time.deltaTime / maxSpreadDuration);
                _currentSpread = spreadCurve.Evaluate(_spreadPower);
            }
            else
            {
                _spreadPower = Mathf.Clamp01(_spreadPower - Time.deltaTime * 0.5f / maxSpreadDuration);
            }

            if (_requestFire)
            {
                if (!_waitToFire && _timeSinceFireRequested < 0.2f) animator.SetTrigger(FireHash);
                else                                                _requestFire = false;
                
                _timeSinceFireRequested += Time.deltaTime;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                ReadyToFire();
            }
        }

        private void OnDisable()
        {
            if (!animator || !weaponMesh) return;
            
            _equipped = false;
            animator.enabled = false;
            
            weaponMesh.SetActive(true);
        }

        public override void PullTrigger()
        {
            _firing = true;
            _requestFire = true;
            _timeSinceFireRequested = 0;
        }

        public override void ReleaseTrigger()
        {
            _firing = false;
        }

        public override void Fire()
        {
            _waitToFire = true;
            
            // Camera feedback
            SceneManager.CameraFeedback.MachineGunFeedback();
            
            // Spawn muzzle effect
            Instantiate(muzzleParticle, muzzle.position, Quaternion.LookRotation(muzzle.forward), muzzle);
            
            var deviation = Mathf.Tan(_currentSpread * Mathf.Deg2Rad);
            var result = WeaponUtility.CrosshairCast(effectiveRange, deviation, Camera.main);

            if (result.hit)
            {
                // Spawn decal
                var decal = Instantiate(bulletDecal, result.col.transform);
                decal.transform.position = result.point + result.normal * 0.25f;
                decal.transform.up = result.normal;
                
                // Notify bullet hit
                EventManager.RaiseBulletHit(result.point, result.normal);

                // Spawn hit particle
                Instantiate(hitParticle, result.point + result.normal * 0.1f, Quaternion.LookRotation(result.normal));
                    
                // Damage
                var receiver = result.col.GetComponentInParent<DamageReceiver>();
                if (receiver) receiver.DealDamage(damage);
            }
        }

        public override void Equip()
        {
            GameLogger.LogDebug($"Equipped weapon - {nameof(AutoMachineGun)}");
            enabled = true;
            StartCoroutine(EquipRoutine());


            IEnumerator EquipRoutine()
            {
                yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Equip"));
                yield return null;
                weaponMesh.SetActive(true);
                yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
                _equipped = true;
            }
        }

        private void ReadyToFire()
        {
            _waitToFire = false;
            
            if (_firing)
            {
                _requestFire = true;
                _timeSinceFireRequested = 0;
            }
        }
    }
}