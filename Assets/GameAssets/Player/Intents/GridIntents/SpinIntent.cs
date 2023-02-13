using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public sealed class SpinIntent : IGridIntent, IContainerProvide
    {
        public GridIntentType IntentType => GridIntentType.None;

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

        public GridValidator AffectedValidation(GridValidator validator, Vector3 position)
        {
            return validator;
        }

        public GridValidator AvaiableValidation(GridValidator validator)
        {
            return validator;
        }
    }
}

