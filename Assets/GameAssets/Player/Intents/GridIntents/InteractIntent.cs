using System.Linq;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public sealed class InteractIntent : IGridIntent, IContainerProvide
    {
        private readonly IWorldCursor worldCursor;
        private readonly ICharacterUnit character;
        private readonly IUnitWorldGridManager gridManager;

        public int ActionPointsCost { get; set; }

        public bool ExecuteImmediatly => false;

        public IDependencyContainer Container { private get; set; }

        public GridIntentType IntentType => GridIntentType.Interact;

        public InteractIntent(
            ActionsConfig config,
            ICharacterSelector selector,
            IWorldCursor worldCursor,
            IUnitWorldGridManager gridManager
        )
        {
            ActionPointsCost = config.GetCost(UnitActionsEnum.INTERACT);
            this.gridManager = gridManager;
            this.worldCursor = worldCursor;

            character = selector.CurrentUnit;
        }

        public IAction Create()
        {
            var position = worldCursor.WorldPosition.Get();
            return Container.Resolve<InteractAction>(
                gridManager
                    .GetUnitsInRange(position, character.UnitConfig.InteractRange)
                    .OfType<IInteractableUnit>()
                    .First()
            );
        }

        public GridValidator AffectedValidation(GridValidator validator, Vector3 position)
        {
            return validator
                .WhereCell(position)
                .WhereUnitIs<IInteractableUnit>();
        }

        public GridValidator AvaiableValidation(GridValidator validator)
        {
            return validator
                .WithRange(character.Transform.Position, character.UnitConfig.InteractRange)
                .WhereUnitIs<IInteractableUnit>();
        }
    }
}

