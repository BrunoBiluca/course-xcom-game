using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public sealed class ShootIntent : IGridIntent, IContainerProvide
    {
        private readonly IWorldCursor worldCursor;
        private readonly ICharacterUnit character;
        public GridIntentType IntentType => GridIntentType.Attack;

        public int ActionPointsCost { get; set; }

        public bool ExecuteImmediatly => false;

        public IDependencyContainer Container { private get; set; }

        public ShootIntent(
            ActionsConfig config,
            ICharacterSelector selector,
            IWorldCursor worldCursor
        )
        {
            ActionPointsCost = config.GetCost(UnitActionsEnum.SHOOT);
            this.worldCursor = worldCursor;

            character = selector.CurrentUnit;
        }

        public IAction Create()
        {
            var position = worldCursor.WorldPosition.Get();
            return Container.Resolve<ShootAction>(
                character,
                position,
                Container.Resolve<IProjectileFactory>(ProjectileFactories.Shoot)
            );
        }

        public GridValidator AvaiableValidation(GridValidator validator)
        {
            return validator
                .WithRange(character.Transform.Position, character.UnitConfig.ShootRange)
                .WhereCanDamageUnit(character.Damageable.Layer);
        }

        public GridValidator AffectedValidation(
           GridValidator validator,
           Vector3 position
       )
        {
            return validator
                .WhereCell(position)
                .WhereCanDamageUnit(character.Damageable.Layer);
        }
    }
}

