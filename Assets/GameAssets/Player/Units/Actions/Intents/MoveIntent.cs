﻿using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.WorldCursors;
using static GameAssets.UnitWorldGridManager;

namespace GameAssets
{
    public sealed class MoveIntent : IGridIntent, IContainerProvide
    {
        private readonly ICharacterSelector selector;
        private readonly IWorldCursor worldCursor;

        public int ActionPointsCost { get; set; }

        public bool ExecuteImmediatly => false;

        public IDependencyContainer Container { private get; set; }

        public MoveIntent(
            ActionsConfig config,
            ICharacterSelector selector,
            IWorldCursor worldCursor
        )
        {
            ActionPointsCost = config.GetCost(UnitActionsEnum.MOVE);
            this.selector = selector;
            this.worldCursor = worldCursor;
        }

        public IAction Create()
        {
            var character = selector.CurrentUnit;
            var position = worldCursor.WorldPosition.Get();
            return Container.Resolve<StepMovementAction>(character, position);
        }

        public void GridValidation()
        {
            var gridManager = Container.Resolve<IUnitWorldGridManager>();
            var character = selector.CurrentUnit;
            gridManager.Validator()
                .WhereIsEmpty()
                .WithDirectRange(character.Transform.Position, character.UnitConfig.MovementRange)
                .Apply();
        }
    }
}

