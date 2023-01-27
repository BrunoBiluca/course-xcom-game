using System;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.Grid;
using UnityFoundation.TurnSystem;

namespace GameAssets
{
    public class UnitsManager
        : MonoBehaviour
        , IUnitsManager
        , IBilucaLoggable
        , IDependencySetup<UnitsManager.Params>
    {
        public class Params
        {
            public LevelSetupConfigSO LevelSetupConfig { get; private set; }
            public UnitGridWorldCursor WorldCursor { get; private set; }
            public UnitWorldGridManager GridManager { get; private set; }
            public ITurnSystem TurnSystem { get; private set; }
            public IActorSelector<IAPActor> ActorSelector { get; private set; }

            public Params(LevelSetupConfigSO levelSetupConfig, UnitGridWorldCursor worldCursor, UnitWorldGridManager gridManager, ITurnSystem turnSystem, IActorSelector<IAPActor> actorSelector)
            {
                LevelSetupConfig = levelSetupConfig;
                WorldCursor = worldCursor;
                GridManager = gridManager;
                TurnSystem = turnSystem;
                ActorSelector = actorSelector;
            }
        }

        private LevelSetupConfigSO levelSetupConfig;
        private UnitGridWorldCursor worldCursor;
        private UnitWorldGridManager gridManager;
        private IActorSelector<IAPActor> actorSelector;
        private ITurnSystem turnSystem;
        private List<ICharacterUnit> units;

        public int CurrentUnitsCount => units.Count;

        public IBilucaLogger Logger { get; set; }

        public event Action OnAllUnitsDied;

        public void Setup(Params parameters)
        {
            levelSetupConfig = parameters.LevelSetupConfig;
            worldCursor = parameters.WorldCursor;
            gridManager = parameters.GridManager;
            actorSelector = parameters.ActorSelector;
            turnSystem = parameters.TurnSystem;
        }

        public void InstantiateUnits()
        {
            SetupUnits();
            turnSystem.OnPlayerTurnEnded += RefillUnitActions;
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
            Debug.Log("sadfsadfsadfsad");
            Logger?.LogHighlight("Instantiating units");
            units = new List<ICharacterUnit>();
            foreach(var unitSetup in levelSetupConfig.Units)
            {
                var unit = Instantiate(unitSetup.prefab).GetComponent<PlayerUnit>();
                unit.Setup(
                    unitSetup.UnitTemplate.UnitConfig,
                    unitSetup.UnitTemplate.SoundEffects,
                    worldCursor
                );

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
