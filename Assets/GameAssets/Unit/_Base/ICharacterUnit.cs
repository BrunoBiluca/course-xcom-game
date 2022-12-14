using GameAssets.ActorSystem;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.HealthSystem;

namespace GameAssets
{

    /// <summary>
    /// Character units placed in the grid
    /// </summary>
    public interface ICharacterUnit : IUnit
    {
        IDamageable Damageable { get; }
        IAPActor Actor { get; }
        UnitConfigTemplate UnitConfigTemplate { get; }
        ITransform RightShoulder { get; }
        AnimatorController AnimatorController { get; }
        ITransform ProjectileStart { get; }
        INavegationAgent TransformNav { get; }
    }
}
