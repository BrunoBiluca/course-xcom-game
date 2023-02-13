using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;
using UnityFoundation.WorldCursors;
using static GameAssets.UnitWorldGridManager;

namespace GameAssets
{
    public sealed class MoveIntent : IGridIntent, IContainerProvide
    {
        private readonly IWorldCursor worldCursor;
        private readonly ICharacterUnit character;

        public int ActionPointsCost { get; set; }

        public bool ExecuteImmediatly => false;

        public IDependencyContainer Container { private get; set; }

        public GridIntentType IntentType => GridIntentType.Movement;

        public MoveIntent(
            ActionsConfig config,
            ICharacterSelector selector,
            IWorldCursor worldCursor
        )
        {
            ActionPointsCost = config.GetCost(UnitActionsEnum.MOVE);
            this.worldCursor = worldCursor;

            character = selector.CurrentUnit;
        }

        public IAction Create()
        {
            var position = worldCursor.WorldPosition.Get();
            return Container.Resolve<StepMovementAction>(character, position);
        }

        public GridValidator AvaiableValidation(GridValidator validator)
        {
            return validator
                .WhereIsEmpty()
                .WithDirectRange(character.Transform.Position, character.UnitConfig.MovementRange);
        }

        public GridValidator AffectedValidation(GridValidator validator, Vector3 position)
        {
            return validator.WhereIsEmpty().WhereCell(position);
        }
    }
}

