using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.SettingsSystem;

namespace GameAssets
{
    /// <summary>
    /// Character units placed in the grid
    /// </summary>
    public interface ICharacterUnit : IDamageableUnit, IDestroyable
    {
        IAPActor Actor { get; }
        UnitConfig UnitConfig { get; }
        ITransform RightShoulder { get; }
        ICharacterAnimatorController AnimatorController { get; }
        ISoundEffectsController SoundEffectsController { get; }
        ITransform ProjectileStart { get; }
        INavegationAgent TransformNav { get; }
        SoundEffects SoundEffects { get; }
    }
}
