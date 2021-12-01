using System;
using System.Collections.Generic;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Enemies
{
    public class AIContext : MonoBehaviour
    {
        [Title("AI Lists")] 
        public AIController[] tPosers;

        [Title("Scene Info")] 
        public DamageReceiver enemy;
        public Transform enemyEye;
        public Transform[] waypoints;
        public LayerMask sightLayers;

        private void Awake()
        {
            if (AiEnemy.Instance)
            {
                enemy = AiEnemy.Instance.DamageReceiver;
                enemyEye = AiEnemy.Instance.EyeTransform;
            }
            else
            {
                AiEnemy.AiEnemySpawned += OnAiEnemySpawned;
            }
        }

        private void OnDestroy()
        {
            AiEnemy.AiEnemySpawned -= OnAiEnemySpawned;
        }

        private void OnAiEnemySpawned(AiEnemy aiEnemy)
        {
            enemy = aiEnemy.DamageReceiver;
            enemyEye = aiEnemy.EyeTransform;
        }
    }
}