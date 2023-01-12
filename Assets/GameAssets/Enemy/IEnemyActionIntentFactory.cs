using GameAssets;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;

public interface IEnemyActionIntentFactory
{
    IAPIntent IntentMove(ICharacterUnit unit, Vector3 position);
    IAPIntent IntentShoot(ICharacterUnit unit, Vector3 position);
}