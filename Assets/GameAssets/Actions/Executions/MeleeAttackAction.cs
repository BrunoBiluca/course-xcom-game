using GameAssets.ActorSystem;
using System;
using UnityEditor.Experimental.GraphView;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.HealthSystem;

namespace GameAssets
{
    public class MeleeAttackAction : IAction
    {
        private readonly ICharacterUnit attacker;
        private readonly ICharacterUnit target;

        public Settings Config { get; private set; }

        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        public MeleeAttackAction(
            Settings config, 
            ICharacterUnit attacker,
            ICharacterUnit target
        )
        {
            Config = config;
            this.attacker = attacker;
            this.target = target;
        }

        public void Execute()
        {
            attacker.Transform.LookAt(target.Transform.Position);

            attacker.AnimatorController.Play(new MeleeAttackAnimation(true));

            attacker.AnimatorController.OnEventTriggered += CalculateDamage;
        }

        private void CalculateDamage(string eventName)
        {
            if(eventName != "damage")
                return;

            target.Damageable.Damage(Config.Damage);
            attacker.AnimatorController.Play(new MeleeAttackAnimation(false));

            attacker.AnimatorController.OnEventTriggered -= CalculateDamage;
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
