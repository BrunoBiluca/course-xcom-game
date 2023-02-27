using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.TurnSystem;

namespace GameAssets
{
    public class EnemiesManager
        : BilucaMono
        , IBilucaLoggable
        , IDependencySetup<
            LevelSetupConfigSO, UnitWorldGridManager, ITurnSystem, IEnemyActionIntentFactory
        >
    {
        private LevelSetupConfigSO levelSetupConfig;
        private UnitWorldGridManager gridManager;
        private IEnemyActionIntentFactory enemyActionIntentFactory;
        private ITurnSystem turnSystem;

        private List<EnemyUnit> enemies = new();

        public int CurrentEnemiesCount => enemies.Count;
        public event Action OnEnemiesDied;

        public void Setup(
            LevelSetupConfigSO levelSetupConfig,
            UnitWorldGridManager gridManager,
            ITurnSystem turnSystem,
            IEnemyActionIntentFactory enemyActionIntentFactory
        )
        {
            this.levelSetupConfig = levelSetupConfig;
            this.gridManager = gridManager;
            this.enemyActionIntentFactory = enemyActionIntentFactory;
            this.turnSystem = turnSystem;
        }

        private void EnemyStartTurn()
        {
            enemies.RemoveAll(e => e == null);
            enemies.ForEach(e => e.Actor.ActionPoints.FullReffil());
            _ = EnemyStartTurnAsync();
        }

        private async Task EnemyStartTurnAsync()
        {
            try
            {
                Logger?.Log("Start enemy turn");
                foreach(var enemy in enemies)
                {
                    Logger?.Log(enemy.Name);
                    await enemy.TakeActions();
                }

                Logger?.Log("Finish enemy turn");
                turnSystem.EndEnemyTurn();
            }
            catch(Exception ex)
            {
                Logger?.Error(ex);
            }
        }

        public void SetupEnemies()
        {
            foreach(var enemy in gridManager.Units.OfType<EnemyUnit>())
            {
                enemy.Setup(gridManager, enemyActionIntentFactory);

                enemy.HealthSystem.OnDied += () => HandleEnemyDestroyed(enemy);

                enemies.Add(enemy);
            }
        }

        public void InstantiateEnemies()
        {
            Logger?.LogHighlight("Instantiating enemy units");
            enemies = new List<EnemyUnit>();
            var count = 1;
            foreach(var enemy in levelSetupConfig.Enemies)
            {
                var newEnemy = Instantiate(enemy.EnemyPrefab).GetComponent<EnemyUnit>();
                newEnemy.Transform.Name = "Enemy " + count++;
                newEnemy.Logger = Logger;
                newEnemy.Setup(
                    gridManager,
                    enemyActionIntentFactory
                );

                newEnemy.Transform.Position = gridManager.Grid
                    .GetCellCenterPosition(
                        new GridCellPositionXZ(enemy.Position.X, enemy.Position.Z)
                );

                newEnemy.Obj.OnObjectDestroyed += () => HandleEnemyDestroyed(newEnemy);

                enemies.Add(newEnemy);
                gridManager.Add(newEnemy);
            }
        }

        private void HandleEnemyDestroyed(EnemyUnit enemy)
        {
            enemies.Remove(enemy);
            if(enemies.IsEmpty())
                OnEnemiesDied?.Invoke();
        }

        public void InstantiateUnits()
        {
            turnSystem.OnEnemyTurnStarted += EnemyStartTurn;
            //InstantiateEnemies();
            SetupEnemies();
        }
    }
}
