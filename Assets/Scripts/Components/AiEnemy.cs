using System;
using UnityEngine;

namespace Components
{
    public class AiEnemy : MonoBehaviour
    {
        private static AiEnemy _instance;
        public static AiEnemy Instance => _instance ? _instance : _instance = FindObjectOfType<AiEnemy>();
        
        public static event Action<AiEnemy> AiEnemySpawned;
        
        #pragma warning disable 649
        [SerializeField] private Transform eyeTransform;
        [SerializeField] private DamageReceiver damageReceiver;
        #pragma warning restore 649

        public Transform EyeTransform => eyeTransform;

        public DamageReceiver DamageReceiver => damageReceiver;

        private void Start()
        {
            AiEnemySpawned?.Invoke(this);
        }
    }
}