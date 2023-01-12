using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.HealthSystem;

namespace GameAssets
{

    /// <summary>
    /// Character units placed in the grid
    /// </summary>
    public interface ICharacterUnit : IUnit, IDestroyable
    {
        IDamageable Damageable { get; }
        IAPActor Actor { get; }
        UnitConfig UnitConfig { get; }
        ITransform RightShoulder { get; }
        IAnimatorController AnimatorController { get; }
        ITransform ProjectileStart { get; }
        INavegationAgent TransformNav { get; }
        IHealthSystem HealthSystem { get; }
    }
}
