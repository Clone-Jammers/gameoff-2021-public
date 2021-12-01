using UnityEngine;

namespace Components
{
    public abstract class DamageReceiver : MonoBehaviour
    {
        public abstract void DealDamage(int damage);
        public abstract void RunOutOfDurability();
    }
}