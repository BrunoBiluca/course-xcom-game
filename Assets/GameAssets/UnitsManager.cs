using UnityEngine;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class UnitsManager : MonoBehaviour
    {
        [SerializeField] private GameObject unitPrefab;
        private LevelSetupConfig levelSetupConfig;
        private IWorldCursor worldCursor;
        private WorldGridXZManager<GridUnitValue> gridManager;

        public void Setup(
            LevelSetupConfig levelSetupConfig,
            IWorldCursor worldCursor,
            WorldGridXZManager<GridUnitValue> gridManager
        )
        {
            this.levelSetupConfig = levelSetupConfig;
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;
            SetupUnits();
        }

        public void SetupUnits()
        {
            foreach(var unitSetup in levelSetupConfig.Units)
            {
                var unit = Instantiate(unitPrefab).GetComponent<UnitMono>();
                unit.Setup(unitSetup.UnitTemplate, worldCursor, gridManager);

                unit.Transform.Position = gridManager.Grid
                    .GetCellCenterPosition(
                        new GridCellPositionXZ(unitSetup.PositionX, unitSetup.PositionZ)
                    );
            }
        }

        public UnitMono[] GetAllUnits()
        {
            return FindObjectsOfType<UnitMono>();
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
