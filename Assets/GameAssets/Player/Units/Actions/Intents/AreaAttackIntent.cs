using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.WorldCursors;
using static GameAssets.UnitWorldGridManager;

namespace GameAssets
{
    public sealed class AreaAttackIntent : IGridIntent, IContainerProvide
    {
        private readonly ICharacterSelector selector;
        private readonly IWorldCursor worldCursor;
        private readonly IUnitWorldGridManager gridManager;

        public int ActionPointsCost { get; set; }

        public bool ExecuteImmediatly => false;

        public IDependencyContainer Container { private get; set; }

        public AreaAttackIntent(
            ActionsConfig config,
            ICharacterSelector selector,
            IWorldCursor worldCursor,
            IUnitWorldGridManager gridManager
        )
        {
            ActionPointsCost = config.GetCost(UnitActionsEnum.GRENADE);
            this.selector = selector;
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;
        }

        public IAction Create()
        {
            var character = selector.CurrentUnit;
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

        public void GridValidation()
        {
            var character = selector.CurrentUnit;
            gridManager
                .Validator()
                .WithRange(character.Transform.Position, character.UnitConfig.GrenadeRange)
                .Apply(GridState.Attack);
        }
    }
}

