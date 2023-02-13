using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class MeleeAttackIntent : IGridIntent, IContainerProvide
    {
        private readonly IUnitWorldGridManager gridManager;
        private readonly IWorldCursor worldCursor;
        private readonly ICharacterUnit character;

        public int ActionPointsCost { get; set; }
        public bool ExecuteImmediatly => false;
        public GridIntentType IntentType => GridIntentType.Attack;

        public IDependencyContainer Container { private get; set; }

        public MeleeAttackIntent(
            ActionsConfig actionsConfig,
            ICharacterSelector selector,
            IUnitWorldGridManager gridManager,
            IWorldCursor worldCursor
        )
        {
            ActionPointsCost = actionsConfig.GetCost(UnitActionsEnum.MELEE);
            this.gridManager = gridManager;
            this.worldCursor = worldCursor;

            character = selector.CurrentUnit;
        }

        public IAction Create()
        {
            var cellValue = gridManager.Grid.GetValue(worldCursor.WorldPosition.Get());
            var targetUnit = cellValue.Units[0] as IDamageableUnit;
            return Container.Resolve<MeleeAttackAction>(character, targetUnit);
        }

        public GridValidator AffectedValidation(
            GridValidator validator, Vector3 position
        )
        {
            return validator
                .WhereCell(position)
                .WhereCanDamageUnit(character.Damageable.Layer);
        }

        public GridValidator AvaiableValidation(GridValidator validator)
        {
            return validator
                .WithRange(
                    character.Transform.Position,
                    character.UnitConfig.MeleeRange
                )
                .WhereCanDamageUnit(character.Damageable.Layer);
        }
    }
}

