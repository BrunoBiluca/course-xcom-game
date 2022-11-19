using System;

namespace GameAssets
{
    public sealed class APUnitActionsFactory : IUnitActionsFactory<IAPUnitAction>
    {
        private readonly IUnitActionsFactory<IUnitAction> unitActionsFactory;

        public APUnitActionsFactory(
            IUnitActionsFactory<IUnitAction> unitActionsFactory
        )
        {
            this.unitActionsFactory = unitActionsFactory;
        }

        public IAPUnitAction Get(UnitActionsEnum action)
        {
            var newAction = unitActionsFactory.Get(action);
            return action switch {
                UnitActionsEnum.SPIN => InstantiateSpin(newAction),
                UnitActionsEnum.MOVE => InstantiateMove(newAction),
                UnitActionsEnum.SHOOT => InstantiateShoot(newAction),
                _ => throw new NotImplementedException(),
            };
        }

        private IAPUnitAction InstantiateShoot(IUnitAction newAction)
        {
            return new APUnitAction(newAction) { ActionPointsCost = 1 };
        }

        private IAPUnitAction InstantiateMove(IUnitAction newAction)
        {
            return new APUnitAction(newAction) { ActionPointsCost = 1 };
        }

        private IAPUnitAction InstantiateSpin(IUnitAction newAction)
        {
            return new APUnitAction(newAction) { ActionPointsCost = 2 };
        }
    }
}
