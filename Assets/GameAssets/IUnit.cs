using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.HealthSystem;

namespace GameAssets
{
    /// <summary>
    /// Base interface for placeable units in the grid
    /// </summary>
    public interface IUnit
    {
        string Name { get; }
        ITransform Transform { get; }

        IDamageable Damageable { get; }

        IAPActor Actor { get; }
        UnitConfigTemplate UnitConfigTemplate { get; }
        ITransform RightShoulder { get; }
        AnimatorController AnimatorController { get; }
        ITransform ProjectileStart { get; }
        INavegationAgent TransformNav { get; }
    }
}
