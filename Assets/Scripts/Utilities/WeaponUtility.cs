using Components.Player;
using Components.Weapons;
using Managers;
using UnityEngine;

namespace Utilities
{
    public static class WeaponUtility
    {
        private static LayerMask HitMask;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            HitMask = LayerMask.GetMask("Default", "Enemy", "Props", "Walls", "Water", "Ground");
        }
        
        public static (bool hit, Vector3 hitPoint, Vector3 normal, Collider col) CrosshairCast(float weaponRange, Camera mainCamera)
        {
            var scPoint = mainCamera.WorldToScreenPoint(SceneManager.PlayerHudController.CrosshairPosition);
            var ray = mainCamera.ScreenPointToRay(scPoint);

            if (Physics.Raycast(ray, out var hit, weaponRange, HitMask))
            {
                return (true, hit.point, hit.normal, hit.collider);
            }
            
            return (false, default, default, default);
        }
        
        public static (bool hit, Vector3 point, Vector3 normal, Collider col) CrosshairCast(float weaponRange, float deviation, Camera mainCamera)
        {
            var hudController = SceneManager.PlayerHudController;
            var head = SceneManager.PlayerCharacterController.Head;
            var deviationVector = head.transform.rotation * Random.insideUnitCircle * deviation;
            var scPoint = mainCamera.WorldToScreenPoint(hudController.CrosshairPosition + deviationVector);
            var ray = mainCamera.ScreenPointToRay(scPoint);

            if (Physics.Raycast(ray, out var hit, weaponRange, HitMask))
            {
                return (true, hit.point, hit.normal, hit.collider);
            }
            
            return (false, default, default, default);
        }

        public static void CrosshairCast(int shellCount, float weaponRange, float spreadAngle,
            Camera mainCamera, out (bool hit, Vector3 point, Vector3 normal, Collider col)[] results)
        {
            // Create central ray cast parameters
            var scPoint = mainCamera.WorldToScreenPoint(SceneManager.PlayerHudController.CrosshairPosition);
            
            // Prepare shell rays
            results = new (bool hit, Vector3 point, Vector3 normal, Collider col)[shellCount];
            var centralRay = mainCamera.ScreenPointToRay(scPoint);

            // Raycast and get hit points
            for (int i = 0; i < shellCount; i++)
            {
                var head = SceneManager.PlayerCharacterController.Head;
                var deviation = head.rotation * Random.insideUnitCircle * spreadAngle;
                var worldPoint = centralRay.GetPoint(weaponRange) + deviation;
                var shellRay = mainCamera.ScreenPointToRay(mainCamera.WorldToScreenPoint(worldPoint));

                if (Physics.Raycast(shellRay, out var hit, weaponRange, HitMask))
                {
                    results[i] = (true, hit.point, hit.normal, hit.collider);
                }
                else
                {
                    results[i] = (false, default, default, default);
                }
            }
        }
    }
}