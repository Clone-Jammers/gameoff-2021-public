using System.Collections.Generic;
using Components.Enemies;
using UnityEngine;

namespace Utilities
{
    public static class AIUtility
    {
        /// <summary>
        /// Checks if position is in sight with a simple angle check and raycast
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static bool IsEnemyInSight(AIController aiController)
        {
            var direction = aiController.Context.enemyEye.position - aiController.Character.EyePosition;

            if (direction.magnitude > aiController.ChaseRange) return false;
            
            direction.Normalize();
            var targetAngle = Mathf.Abs(Vector3.SignedAngle(aiController.Character.Forward, direction, Vector3.up));

            if (targetAngle > aiController.LosAngle) return false;

            var sightLayers = aiController.Context.sightLayers;
            if (Physics.Raycast(aiController.Character.EyePosition, direction, out var hit, aiController.ChaseRange, sightLayers.value))
            {
                return hit.transform == aiController.Context.enemy;
            }

            return false;
        }

        public static bool IsEnemyInChaseRange(AIController controller)
        {
            return Vector3.Distance(controller.Character.Position, controller.Context.enemy.transform.position) <
                   controller.ChaseRange;
        }

        public static bool IsEnemyInChaseSoundRange(AIController controller)
        {
            return Vector3.Distance(controller.Character.Position, controller.Context.enemy.transform.position) <
                   controller.HearRange;
        }

        public static bool IsEnemyInAttackRange(AIController controller)
        {
            return Vector3.Distance(controller.Character.Position, controller.Context.enemy.transform.position) <
                   controller.AttackRange;
        }

        public static void DrawRangeGizmo(Vector3 position, float radius, Color color)
        {
            #if UNITY_EDITOR
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.DrawWireDisc(position, Vector3.up, radius);
            #endif
        }

        public static void DrawDestinationGizmo(Vector3 position, Vector3 destination, Color color)
        {
            #if UNITY_EDITOR
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.DrawLine(position, destination);
            #endif
        }
    }
}