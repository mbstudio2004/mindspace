using Nocci.Character.Structs;
using UnityEngine.Events;

namespace Nocci.Character.Interfaces
{
    public interface IHealthManager : ICharacterModule
    {
        bool IsAlive { get; }

        float Health { get; }
        float PrevHealth { get; }
        float MaxHealth { get; set; }

        event UnityAction<float, DamageContext> DamageTakenFullContext;

        event UnityAction<float> DamageTaken;
        event UnityAction<float> HealthRestored;
        event UnityAction<float> MaxHealthChanged;

        event UnityAction Death;
        event UnityAction Respawn;


        void RestoreHealth(float healthRestore);
        void ReceiveDamage(float damage);
        void ReceiveDamage(float damage, DamageContext dmgContext);
    }
}