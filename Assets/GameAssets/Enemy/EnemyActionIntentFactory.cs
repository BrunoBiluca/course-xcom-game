using GameAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;

public class EnemyActionIntentFactory : IEnemyActionIntentFactory
{
    private readonly UnitWorldGridManager gridManager;
    private readonly ProjectileFactory projectileFactory;

    public EnemyActionIntentFactory(
        UnitWorldGridManager gridManager,
        ProjectileFactory projectileFactory
    )
    {
        this.gridManager = gridManager;
        this.projectileFactory = projectileFactory;
    }

    public IAPIntent IntentShoot(ICharacterUnit unit, Vector3 position)
    {
        return new EnemyShootIntent(unit, position, gridManager, projectileFactory);
    }

    public IAPIntent IntentMove(ICharacterUnit unit, Vector3 position)
    {
        return new EnemyMoveIntent(unit, position, gridManager);
    }

}
