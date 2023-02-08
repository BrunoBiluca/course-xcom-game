using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using static GameAssets.UnitWorldGridManager;

namespace GameAssets
{
    public sealed class SpinIntent : IGridIntent, IContainerProvide
    {
        public GridState GridState => GridState.Movement;

        public int ActionPointsCost { get; set; }

        public bool ExecuteImmediatly => true;

        public IDependencyContainer Container { private get; set; }

        public SpinIntent(ActionsConfig config)
        {
            ActionPointsCost = config.GetCost(UnitActionsEnum.SPIN);
        }

        public IAction Create()
        {
            var selector = Container.Resolve<ICharacterSelector>();
            return Container.Resolve<SpinUnitAction>(selector.CurrentUnit.Transform);
        }

        public void GridValidation()
        {
        }
    }
}

