using GameAssets.ActorSystem;
using UnityEngine;
using UnityFoundation.Code.Grid;
using UnityFoundation.TurnSystem;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class UnitsManager : MonoBehaviour
    {
        private LevelSetupConfig levelSetupConfig;
        private IWorldCursor worldCursor;
        private UnitWorldGridManager gridManager;
        private IActorSelector<IAPActor> actorSelector;

        public void Setup(
            LevelSetupConfig levelSetupConfig,
            IWorldCursor worldCursor,
            UnitWorldGridManager gridManager,
            ITurnSystem turnSystem,
            IActorSelector<IAPActor> actorSelector
        )
        {
            this.levelSetupConfig = levelSetupConfig;
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;

            this.actorSelector = actorSelector;

            turnSystem.OnPlayerTurnEnded += RefillUnitActions;
            SetupUnits();
        }

        private void RefillUnitActions()
        {
            actorSelector.UnselectUnit();
            foreach(var u in GetAllUnits())
            {
                u.ActionPoints.FullReffil();
            }
        }

        public void SetupUnits()
        {
            foreach(var unitSetup in levelSetupConfig.Units)
            {
                var unit = Instantiate(unitSetup.prefab).GetComponent<TrooperUnit>();
                unit.Setup(unitSetup.UnitTemplate, worldCursor);

                unit.Transform.Position = gridManager.Grid
                    .GetCellCenterPosition(
                        new GridCellPositionXZ(unitSetup.Position.X, unitSetup.Position.Z)
                    );

                gridManager.Add(unit);
            }
        }

        public TrooperUnit[] GetAllUnits()
        {
            return FindObjectsOfType<TrooperUnit>();
        }

        public void DestroyAllUnits()
        {
            var units = GetAllUnits();

            foreach(var u in units)
            {
                Destroy(u.gameObject);
            }
        }

        public void ResetUnits()
        {
            DestroyAllUnits();
            SetupUnits();
        }
    }
}
