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
        private UnitWorldGridXZManager gridManager;

        public void Setup(
            LevelSetupConfig levelSetupConfig,
            IWorldCursor worldCursor,
            UnitWorldGridXZManager gridManager,
            ITurnSystem turnSystem
        )
        {
            this.levelSetupConfig = levelSetupConfig;
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;

            turnSystem.OnPlayerTurnEnded += RefillUnitActions;
            SetupUnits();
        }

        private void RefillUnitActions()
        {
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
                unit.Setup(unitSetup.UnitTemplate, worldCursor, gridManager);

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
