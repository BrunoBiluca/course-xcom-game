using System;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code.Grid;
using UnityFoundation.TurnSystem;

namespace GameAssets
{
    public class UnitsManager : MonoBehaviour, IUnitsManager
    {
        private LevelSetupConfig levelSetupConfig;
        private UnitGridWorldCursor worldCursor;
        private UnitWorldGridManager gridManager;
        private IActorSelector<IAPActor> actorSelector;

        private List<ICharacterUnit> units;

        public int CurrentUnitsCount => units.Count;

        public event Action OnAllUnitsDied;

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
                u.Actor.ActionPoints.FullReffil();
            }
        }

        public void SetupUnits()
        {
            units = new List<ICharacterUnit>();
            foreach(var unitSetup in levelSetupConfig.Units)
            {
                var unit = Instantiate(unitSetup.prefab).GetComponent<TrooperUnit>();
                unit.Setup(unitSetup.UnitTemplate.UnitConfig, worldCursor);

                unit.Transform.Position = gridManager.Grid
                    .GetCellCenterPosition(
                        new GridCellPositionXZ(unitSetup.Position.X, unitSetup.Position.Z)
                    );

                unit.Obj.OnObjectDestroyed += () => {
                    units.Remove(unit);
                    HandleUnitDetroy();
                };

                gridManager.Add(unit);
                units.Add(unit);
            }
        }

        private void HandleUnitDetroy()
        {
            if(CurrentUnitsCount == 0)
                OnAllUnitsDied?.Invoke();
        }

        public ICharacterUnit[] GetAllUnits()
        {
            return units.ToArray();
        }

        public void DestroyAllUnits()
        {
            foreach(var u in units)
                u.Destroy();

            units.Clear();
        }

        public void ResetUnits()
        {
            DestroyAllUnits();
            SetupUnits();
        }
    }
}
