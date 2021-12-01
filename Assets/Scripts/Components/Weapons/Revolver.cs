using System.Collections;
using Components.Logger;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities;

namespace Components.Weapons
{
    public class Revolver : Weapon
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
        #pragma warning restore 649

        private bool _equipped;
        private bool _requestFire;
        private bool _waitForCharge;
        
        private float _timeSinceFireRequested;

        private void Awake()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            _equipped = false;
            _waitForCharge = false;
            _requestFire = false;
            _timeSinceFireRequested = 0;

            animator.enabled = true;
            weaponMesh.SetActive(false);
        }

        private void Update()
        {
            if (!_equipped) return;
            
            if (_requestFire)
            {
                if (_timeSinceFireRequested > 0.15f)
                {
                    _requestFire = false;
                    return;
                }
                
                _timeSinceFireRequested += Time.deltaTime;
                
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
            if (_waitForCharge) return;

            // Modify state
            _requestFire = false;
            _waitForCharge = true;
            
            // Play fire animation
            animator.SetTrigger(FireHash);

            // Fire using crosshair and calculate hits and misses
            var result = WeaponUtility.CrosshairCast(effectiveRange, 0, Camera.main);
            
            // Camera feedback
            SceneManager.CameraFeedback.ShotgunFeedback();
            
            // Spawn muzzle effect
            Instantiate(muzzleParticle, muzzle.position, Quaternion.LookRotation(muzzle.forward), muzzle);

            // Log hits and misses
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
            GameLogger.LogDebug($"Equipped weapon - {nameof(Revolver)}");
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