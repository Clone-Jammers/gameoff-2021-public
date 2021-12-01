using Components.Logger;
using DG.Tweening;
using KinematicCharacterController;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Components.ChaosMode
{
    public class TeleportToAmmo : MonoBehaviour
    {
        #pragma warning disable 649
        [Range(0, 1)]
        [SerializeField] private float teleportProbability;
        #pragma warning restore 649
    
        private float _t;
        private bool _available;
        private Vector3 _position;
        private KinematicCharacterMotor _motor;

        private void Awake()
        {
            _motor = GetComponent<KinematicCharacterMotor>();
            SceneManager.TeleportToAmmo = this;
            enabled = false;
        }

        private void OnEnable()
        {
            EventManager.BulletHit += OnBulletHit;
        }

        private void LateUpdate()
        {
            _t += Time.deltaTime;
            if (_available && _t > 1.25f)
            {
                _t %= 1.25f;
                if (Random.Range(0f, 1f) < teleportProbability)
                {
                    _motor.enabled = false;
                    transform.DOMove(_position, 0.25f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            _motor.SetPosition(_position);
                            _motor.enabled = true;
                        });
                    
                    GameLogger.LogError("USER position changed anormaly!");
                }
                _available = false;
            }
        }

        private void OnDisable()
        {
            EventManager.BulletHit -= OnBulletHit;
        }

        private void OnBulletHit(Vector3 position, Vector3 normal)
        {
            _available = true;
            _position = position + normal;
        }
    }
}