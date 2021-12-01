using UnityEngine;

namespace Managers
{
    public static class EventManager
    {
        public static event BulletHitDelegate BulletHit;
        public static event PlayerDiedDelegate PlayerDied;

        public static void RaiseBulletHit(Vector3 hitPoint, Vector3 normal)
        {
            BulletHit?.Invoke(hitPoint, normal);
        }

        public static void RaisePlayerDied()
        {
            PlayerDied?.Invoke();
        }
    }
    
    public delegate void BulletHitDelegate(Vector3 position, Vector3 normal);

    public delegate void PlayerDiedDelegate();
}