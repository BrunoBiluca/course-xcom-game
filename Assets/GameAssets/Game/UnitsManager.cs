using GameAssets.ActorSystem;
using System;
using UnityEngine;
using UnityFoundation.Code.Grid;
using UnityFoundation.TurnSystem;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class UnitsManager : MonoBehaviour
    {
        private LevelSetupConfig levelSetupConfig;
        private UnitGridWorldCursor worldCursor;
        private UnitWorldGridManager gridManager;
        private IActorSelector<IAPActor> actorSelector;

        public int CurrentUnitsCount { get; private set; }

        public event Action OnUnitsDied;

        public void Setup(
            LevelSetupConfig levelSetupConfig,
            UnitGridWorldCursor worldCursor,
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

                unit.Obj.OnObjectDestroyed += HandleUnitDetroy;
                gridManager.Add(unit);
            }

            CurrentUnitsCount = levelSetupConfig.Units.Length;
        }

        private void HandleUnitDetroy()
        {
            CurrentUnitsCount--;
            if(CurrentUnitsCount == 0)
                OnUnitsDied?.Invoke();
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
