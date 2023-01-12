using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class EnemyMoveIntent : IAPIntent
    {
        private ICharacterUnit unit;
        private Vector3 position;
        private UnitWorldGridManager gridManager;

        public int ActionPointsCost { get; set; } = 1;

        public bool ExecuteImmediatly => true;

        public EnemyMoveIntent(
            ICharacterUnit unit,
            Vector3 position,
            UnitWorldGridManager gridManager
        )
        {
            this.unit = unit;
            this.position = position;
            this.gridManager = gridManager;
        }

        public IAction Create()
        {
            return new StepMoveUnitAction(
                unit, gridManager, position, AsyncProcessor.I
            ) {
                Logger = UnityDebug.I
            };
        }
    }
}
