using System;
using Components.Logger;
using DG.Tweening;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Components.ChaosMode
{
    public class CrosshairJuker : MonoBehaviour
    {
        #pragma warning disable 649
        [SerializeField] private float speed;
        [SerializeField] private float range;
        #pragma warning restore 649

        private Tweener _tween;
        private Vector3 _currentGoal;
        private float _duration;

        private void Awake()
        {
            SceneManager.Juker = this;
            enabled = false;
        }

        private void OnEnable()
        {
            TweenToGoal();
            GameLogger.LogError("Can't find screen center position for crosshair.");
        }

        private void OnDisable()
        {
            if (_tween != null && _tween.IsActive())
            {
                _tween.Kill();
                GameLogger.LogInfo("Screen center found.");
            }
        }

        private void TweenToGoal()
        {
            GenerateRandomPoint();
            _tween = transform.DOLocalMove(_currentGoal, _duration).SetEase(Ease.Linear).OnComplete(TweenToGoal);
        }

        private void GenerateRandomPoint()
        {
            var deviation = Random.insideUnitCircle;
            _currentGoal = transform.localPosition + (Vector3)deviation;
            _currentGoal = Vector3.ClampMagnitude(_currentGoal, range);
            _duration = Vector3.Distance(_currentGoal, Vector3.zero);
        }
    }
}