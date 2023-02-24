using GameAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;

public class EnemyActionIntentFactory : IEnemyActionIntentFactory, IContainerProvide
{
    private readonly UnitWorldGridManager gridManager;

    public IDependencyContainer Container { private get; set; }

    public EnemyActionIntentFactory(
        UnitWorldGridManager gridManager
    )
    {
        this.gridManager = gridManager;
    }

    public IAPIntent IntentShoot(ICharacterUnit unit, Vector3 position)
    {
        return new EnemyShootIntent(
            unit,
            position,
            gridManager,
            Container.Resolve<IProjectileFactory>(ProjectileFactories.WerewolfShot)
        );
    }

    public IAPIntent IntentMove(ICharacterUnit unit, Vector3 position)
    {
        return new EnemyMoveIntent(unit, position, gridManager);
    }

}
