using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public sealed class AreaAttackIntent : IGridIntent, IContainerProvide
    {
        private readonly IWorldCursor worldCursor;
        private readonly ICharacterUnit character;

        public int ActionPointsCost { get; set; }

        public bool ExecuteImmediatly => false;

        public IDependencyContainer Container { private get; set; }

        public GridIntentType IntentType => GridIntentType.Attack;

        public AreaAttackIntent(
            ActionsConfig config,
            ICharacterSelector selector,
            IWorldCursor worldCursor
        )
        {
            //ActionPointsCost = config.GetCost(UnitActionsEnum.GRENADE);
            ActionPointsCost = config.GetCost(UnitActionsEnum.METEOR);
            this.worldCursor = worldCursor;

            character = selector.CurrentUnit;
        }

        public IAction Create()
        {
            var position = worldCursor.WorldPosition.Get();

            //return Container.Resolve<ThrowGrenadeAction>(
            //    new AreaAttackSettings() {
            //        ExplosionRange = character.UnitConfig.ExplosionRange,
            //        Damage = character.UnitConfig.GrenadeDamage
            //    },
            //    character.ProjectileStart.Position,
            //    position,
            //    Container.Resolve<IProjectileFactory>(ProjectileFactories.Grenade)
            //);

            return Container.Resolve<MeteorAttackAction>(
                new AreaAttackSettings() {
                    ExplosionRange = character.UnitConfig.ExplosionRange,
                    Damage = character.UnitConfig.GrenadeDamage
                },
                character,
                position,
                Container.Resolve<IProjectileFactory>(ProjectileFactories.Meteor)
            );
        }

        public GridValidator AvaiableValidation(GridValidator validator)
        {
            return validator
                .WithRange(character.Transform.Position, character.UnitConfig.AreaAttackRange);
        }

        public GridValidator AffectedValidation(
            GridValidator validator,
            Vector3 position
        )
        {
            return validator.WithRange(position, character.UnitConfig.ExplosionRange);
        }
    }
}

