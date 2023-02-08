using UnityFoundation.Code;

namespace GameAssets
{
    public class PlayerUnitIntentsModule : IDependencyModule
    {
        public void Register(IDependencyBinder binder)
        {
            binder.Register<IGridIntent, AreaAttackIntent>(UnitActionsEnum.GRENADE);
            binder.Register<IGridIntent, SpinIntent>(UnitActionsEnum.SPIN);
            binder.Register<IGridIntent, ShootIntent>(UnitActionsEnum.SHOOT);
            binder.Register<IGridIntent, InteractIntent>(UnitActionsEnum.INTERACT);
            binder.Register<IGridIntent, MeleeAttackIntent>(UnitActionsEnum.MELEE);
            binder.Register<IGridIntent, MoveIntent>(UnitActionsEnum.MOVE);
            binder.Register<UnitIntentsFactory>();
        }
    }
}

