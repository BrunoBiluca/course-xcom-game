using System;
using UnityFoundation.CharacterSystem.ActorSystem;

namespace GameAssets
{
    public class MeleeAttackAction : IAction
    {
        private readonly ICharacterUnit attacker;
        private readonly IDamageableUnit target;

        public Settings Config { get; private set; }

        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        public MeleeAttackAction(
            Settings config,
            ICharacterUnit attacker,
            IDamageableUnit target
        )
        {
            Config = config;
            this.attacker = attacker;
            this.target = target;
        }

        public void Execute()
        {
            new ActionDecorator(this)
                .LookAtTarget(
                    attacker, 
                    target.Transform.Position, 
                    PlayMeleeAnimation
            );
        }

        private void PlayMeleeAnimation()
        {
            attacker.AnimatorController.OnEventTriggered += CalculateDamage;
            attacker.AnimatorController.Play(new MeleeAttackAnimation());
        }

        private void CalculateDamage(UnitAnimationEvents eventName)
        {
            if(!Equals(eventName, UnitAnimationEvents.MELEE))
                return;

            attacker.SoundEffectsController.Play(attacker.SoundEffects.Melee);
            target.Damageable.Damage(Config.Damage);

            attacker.AnimatorController.OnEventTriggered -= CalculateDamage;
            OnFinishAction?.Invoke();
        }

        public struct Settings
        {
            public int Damage { get; private set; }

            public Settings(int damage)
            {
                Damage = damage;
            }
        }
    }
}
