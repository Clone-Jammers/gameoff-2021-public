using Sirenix.OdinInspector;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu(menuName = "Game/Stats")]
    public class Stats : ScriptableObject
    {
        [Title("Combat Values")]
        public int damage;
        public int health;
        public float attackSpeed;
        
        [Title("Movement")]
        public float movementSpeed;
        public float turningSpeed;
        
        [Title("AI Behaviour Config")]
        public float attackRange;
        public float chaseRange;
        public float hearRange;
        public float losAngle;
        public float aggressiveness;
    }
}