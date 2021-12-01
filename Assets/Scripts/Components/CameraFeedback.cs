using System;
using System.Collections;
using DG.Tweening;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Components
{
    public class CameraFeedback : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private float maxFeedback;
        [SerializeField] private float shotgunFeedback;
        [SerializeField] private float machineGunFeedback;
        [SerializeField] private float returnSpeed;
        [SerializeField] private float maxHeadbob;
        [SerializeField] private float headBobFeedback;
#pragma warning restore 649


        private float _period;

        private void Awake()
        {
            SceneManager.CameraFeedback = this;
        }

        public void ShotgunFeedback()
        {
            if (DOTween.IsTweening(transform))
            {
                DOTween.Kill(transform);
            }

            var accumulated = transform.localPosition + new Vector3(0.7f, .4f).normalized * shotgunFeedback;
            accumulated = Vector3.ClampMagnitude(accumulated, maxFeedback);

            transform.DOLocalMove(accumulated, 0.05f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                var returnDistance = transform.localPosition.magnitude;
                var returnDuration = returnDistance / returnSpeed;

                transform.DOLocalMove(Vector3.zero, returnDuration).SetEase(Ease.Linear);
            });
        }

        public void MachineGunFeedback()
        {
            if (DOTween.IsTweening(transform))
            {
                DOTween.Kill(transform);
            }

            var accumulated = transform.localPosition + (Vector3)Random.insideUnitCircle * machineGunFeedback;
            accumulated = Vector3.ClampMagnitude(accumulated, maxFeedback);

            transform.DOLocalMove(accumulated, 0.05f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                var returnDistance = transform.localPosition.magnitude;
                var returnDuration = returnDistance / returnSpeed;

                transform.DOLocalMove(Vector3.zero, returnDuration).SetEase(Ease.Linear);
            });
        }

        public void HeadBob(float magnitude)
        {
            if (DOTween.IsTweening(transform))
            {
                DOTween.Kill(transform);
            }

            _period += Time.deltaTime * 10;
            _period %= 180;

            var sinWave = Mathf.Sin(_period);

            var accumulated = transform.localPosition + sinWave * Vector3.up * headBobFeedback;
            accumulated = Vector3.ClampMagnitude(accumulated * magnitude, maxHeadbob);

            transform.DOLocalMove(accumulated, 0.1f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                var returnDistance = transform.localPosition.magnitude;
                var returnDuration = returnDistance / returnSpeed;

                transform.DOLocalMove(Vector3.zero, returnDuration).SetEase(Ease.Linear);
            });
        }
    }
}