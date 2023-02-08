using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.WorldCursors;
using static GameAssets.UnitWorldGridManager;

namespace GameAssets
{
    public sealed class ThrowGrenadeIntent : IGridIntent, IContainerProvide
    {
        private readonly ICharacterSelector selector;
        private readonly IWorldCursor worldCursor;
        private readonly IUnitWorldGridManager gridManager;

        public int ActionPointsCost { get; set; }

        public bool ExecuteImmediatly => false;

        public IDependencyContainer Container { private get; set; }

        public ThrowGrenadeIntent(
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

            return Container.Resolve<ThrowGrenadeAction>(
                new ThrowGrenadeAction.Settings() {
                    ExplosionRange = character.UnitConfig.ExplosionRange,
                    Damage = character.UnitConfig.GrenadeDamage
                },
                character.ProjectileStart.Position,
                position,
                Container.Resolve<IProjectileFactory>(ProjectileFactories.Grenade)
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

