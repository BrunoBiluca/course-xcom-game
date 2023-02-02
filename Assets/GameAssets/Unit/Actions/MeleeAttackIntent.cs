using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.HealthSystem;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class MeleeAttackIntent : IGridIntent, IContainerProvide
    {
        private readonly IActorSelector<ICharacterUnit> selector;
        private readonly IUnitWorldGridManager gridManager;
        private readonly IWorldCursor worldCursor;

        public int ActionPointsCost { get; set; }

        public bool ExecuteImmediatly => false;

        public IDependencyContainer Container { private get; set; }

        public MeleeAttackIntent(
            ActionsConfig actionsConfig,
            IActorSelector<ICharacterUnit> selector,
            IUnitWorldGridManager gridManager,
            IWorldCursor worldCursor
        )
        {
            ActionPointsCost = actionsConfig.GetCost(UnitActionsEnum.MELEE);
            this.selector = selector;
            this.gridManager = gridManager;
            this.worldCursor = worldCursor;
        }

        public IAction Create()
        {
            var cellValue = gridManager.Grid.GetValue(worldCursor.WorldPosition.Get());
            var targetUnit = cellValue.Units[0] as ICharacterUnit;
            return Container.Resolve<MeleeAttackAction>(selector.CurrentUnit, targetUnit);
        }

        public void GridValidation()
        {
            gridManager.Validator()
                    .WithRange(
                        selector.CurrentUnit.Transform.Position,
                        selector.CurrentUnit.UnitConfig.MeleeRange
                    )
                    .WhereUnit((unit) => {
                        if(unit is not ICharacterUnit characterUnit)
                            return false;

                        return DamageableLayerManager.I
                            .LayerCanDamage(
                                selector.CurrentUnit.Damageable.Layer,
                                characterUnit.Damageable.Layer
                            );
                    }).Apply(UnitWorldGridManager.GridState.Attack);
        }
    }
}

