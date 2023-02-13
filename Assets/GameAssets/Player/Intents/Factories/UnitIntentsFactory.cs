using UnityFoundation.Code;

namespace GameAssets
{
    public sealed class UnitIntentsFactory : IContainerProvide
    {
        public IDependencyContainer Container { private get; set; }

        public IGridIntent Get(UnitActionsEnum action)
        {
            return Container.Resolve<IGridIntent>(action);
        }
    }
}

