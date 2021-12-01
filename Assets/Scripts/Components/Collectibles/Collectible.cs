using System;
using DG.Tweening;
using UnityEngine;

namespace Components.Collectibles
{
    public delegate void CollectDelegate(Collectible collectible);
    
    public abstract class Collectible : MonoBehaviour
    {
        public static event CollectDelegate Collected;
        
        #pragma warning disable 649
        [SerializeField] private float deceleration;
        [SerializeField] private Rigidbody body;
        [SerializeField] private Collider trigger;
        #pragma warning restore 649

        private void OnEnable()
        {
            trigger.enabled = false;
        }

        private void FixedUpdate()
        {
            if (body.velocity.magnitude > 0.05f)
            {
                var clampMagnitude = body.velocity.magnitude - deceleration * Time.fixedDeltaTime;
                var clampedMagnitude = Mathf.Clamp(clampMagnitude, 0, float.MaxValue);
                
                body.velocity = Vector3.ClampMagnitude(body.velocity, clampedMagnitude);
            }
            else if (!body.isKinematic)
            {
                trigger.enabled = true;
                body.isKinematic = true;
                transform.DOLocalRotate(new Vector3(0, 180, 0), 0.5f)
                    .SetLoops(-1, LoopType.Incremental)
                    .SetEase(Ease.Linear)
                    .SetRelative(true);
            }
        }

        public void Collect()
        {
            if (transform && DOTween.IsTweening(transform))
                transform.DOKill();
            
            OnCollect();
            Collected?.Invoke(this);
        }

        public void Launch(Vector3 spawnVelocity)
        {
            body.velocity = spawnVelocity;
            body.isKinematic = false;
        }

        protected abstract void OnCollect();
    }
}