using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.HealthSystem;
using UnityFoundation.WorldCursors;
using static GameAssets.UnitWorldGridManager;

namespace GameAssets
{
    public sealed class ShootIntent : IGridIntent, IContainerProvide
    {
        private readonly IActorSelector<ICharacterUnit> selector;
        private readonly IWorldCursor worldCursor;

        public GridState GridState => GridState.None;

        public int ActionPointsCost { get; set; }

        public bool ExecuteImmediatly => false;

        public IDependencyContainer Container { private get; set; }

        public ShootIntent(
            ActionsConfig config,
            IActorSelector<ICharacterUnit> selector,
            IWorldCursor worldCursor,
            IUnitWorldGridManager gridManager
        )
        {
            ActionPointsCost = config.GetCost(UnitActionsEnum.SHOOT);
            this.selector = selector;
            this.worldCursor = worldCursor;
        }

        public IAction Create()
        {
            var character = selector.CurrentUnit;
            var position = worldCursor.WorldPosition.Get();
            return Container.Resolve<ShootAction>(
                character,
                position,
                Container.Resolve<IProjectileFactory>(ProjectileFactories.Shoot)
            );
        }

        public void GridValidation()
        {
            var gridManager = Container.Resolve<IUnitWorldGridManager>();
            var character = selector.CurrentUnit;
            gridManager
                .Validator()
                .WithRange(character.Transform.Position, character.UnitConfig.ShootRange)
                .WhereUnit((unit) => {
                    if(unit is not ICharacterUnit characterUnit)
                        return false;

                    return DamageableLayerManager.I
                        .LayerCanDamage(
                            character.Damageable.Layer,
                            characterUnit.Damageable.Layer
                        );
                })
                .Apply(GridState.Attack);
        }
    }
}

