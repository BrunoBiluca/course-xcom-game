using System;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;

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
            var targetPos = target.Transform.Position;
            attacker.Transform.LookAt(new Vector3(targetPos.x, 0f, targetPos.z));

            if(attacker.RightShoulder != null)
            {
                VisibilityHandlerSingleton.I.Hide();
                CameraManager.I.ShowActionCamera(attacker.RightShoulder.Position, targetPos);
                AsyncProcessor.I.ProcessAsync(PlayMeleeAnimation, 1f);
                return;
            }

            PlayMeleeAnimation();
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

            CameraManager.I.HideActionCamera(1f);

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
