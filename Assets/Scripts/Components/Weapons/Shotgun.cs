using System;
using System.Collections;
using System.Text;
using Components.Logger;
using DarkTonic.MasterAudio;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities;

namespace Components.Weapons
{    
    public class Shotgun : Weapon
    {
        private static readonly int FireHash = Animator.StringToHash("fire");
        
        #pragma warning disable 649
        [SerializeField] private float fireRate;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform muzzle;
        [SerializeField] private GameObject weaponMesh;
        
        [Title("VFX")]
        [SerializeField] private GameObject bulletDecal;
        [SerializeField] private GameObject hitParticle;
        [SerializeField] private GameObject muzzleParticle;

        [Title("Parameters")] 
        [SerializeField] private int damage;
        [SerializeField] private float spreadAngle;
        [SerializeField] private float effectiveRange;
        [SerializeField] private int grapeShotCount;
        #pragma warning restore 649

        private bool _equipped;
        private bool _requestFire;
        private bool _waitForCharge;
        
        private float _fireCooldown;
        private float _timeSinceFireRequested;

        private void Awake()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            _equipped = false;
            _waitForCharge = false;
            _fireCooldown = 0;
            _requestFire = false;
            _timeSinceFireRequested = 0;

            animator.enabled = true;
            weaponMesh.SetActive(false);
        }

        private void Update()
        {
            if (!_equipped) return;
            
            if (_fireCooldown > 0)
            {
                _fireCooldown -= Time.deltaTime;
            }
            
            if (_requestFire)
            {
                if (_timeSinceFireRequested > 0.15f)
                {
                    _requestFire = false;
                    return;
                }
                
                _timeSinceFireRequested += Time.deltaTime;

                if (_fireCooldown > 0) return;

                Fire();
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
            _requestFire = true;
            _timeSinceFireRequested = 0;
        }

        public override void ReleaseTrigger()
        {
            
        }

        public override void Fire()
        {
            if (_waitForCharge || _fireCooldown > 0) return;

            // Modify state
            _requestFire = false;
            _waitForCharge = true;
            _fireCooldown = 1 / fireRate;
            
            // Play fire animation
            animator.SetTrigger(FireHash);

            // Fire using crosshair and calculate hits and misses
            WeaponUtility.CrosshairCast(
                grapeShotCount,
                effectiveRange,
                spreadAngle,
                Camera.main,
                out var launchResults
            );
            
            // Camera feedback
            SceneManager.CameraFeedback.ShotgunFeedback();
            
            // Spawn muzzle effect
            Instantiate(muzzleParticle, muzzle.position, Quaternion.LookRotation(muzzle.forward), muzzle);

            // Log hits and misses
            foreach (var result in launchResults)
            {
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
        }

        public override void Equip()
        {
            GameLogger.LogDebug($"Equipped weapon - {nameof(Shotgun)}");
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
            _waitForCharge = false;
        }
    }
}