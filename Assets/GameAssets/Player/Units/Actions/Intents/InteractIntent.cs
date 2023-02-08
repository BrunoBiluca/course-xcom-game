using System.Linq;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.WorldCursors;
using static GameAssets.UnitWorldGridManager;

namespace GameAssets
{
    public sealed class InteractIntent : IGridIntent, IContainerProvide
    {
        private readonly ICharacterSelector selector;
        private readonly IWorldCursor worldCursor;
        private readonly IUnitWorldGridManager gridManager;

        public int ActionPointsCost { get; set; }

        public bool ExecuteImmediatly => false;

        public IDependencyContainer Container { private get; set; }

        public InteractIntent(
            ActionsConfig config,
            ICharacterSelector selector,
            IWorldCursor worldCursor,
            IUnitWorldGridManager gridManager
        )
        {
            ActionPointsCost = config.GetCost(UnitActionsEnum.INTERACT);
            this.selector = selector;
            this.gridManager = gridManager;
            this.worldCursor = worldCursor;
        }

        public IAction Create()
        {
            var character = selector.CurrentUnit;
            var position = worldCursor.WorldPosition.Get();
            return Container.Resolve<InteractAction>(
                gridManager
                    .GetUnitsInRange(position, character.UnitConfig.InteractRange)
                    .OfType<IInteractableUnit>()
                    .First()
            );
        }

        public void GridValidation()
        {
            var character = selector.CurrentUnit;
            gridManager.Validator()
                .WithRange(character.Transform.Position, character.UnitConfig.InteractRange)
                .WhereUnitIs<IInteractableUnit>()
                .Apply(GridState.Interact);
        }
    }
}

