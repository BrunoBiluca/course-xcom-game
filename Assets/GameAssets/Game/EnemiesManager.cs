using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.TurnSystem;

namespace GameAssets
{
    public class EnemiesManager : BilucaMono, IBilucaLoggable
    {
        private LevelSetupConfigSO levelSetupConfig;
        private UnitWorldGridManager gridManager;
        private IEnemyActionIntentFactory enemyActionIntentFactory;
        private ITurnSystem turnSystem;

        private List<EnemyUnit> enemies;

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
            turnSystem.OnEnemyTurnStarted += EnemyStartTurn;
            SetupEnemies();
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
            enemies = new List<EnemyUnit>();
            var count = 1;
            foreach(var enemy in levelSetupConfig.Enemies)
            {
                var newEnemy = Instantiate(enemy.EnemyPrefab).GetComponent<EnemyUnit>();
                newEnemy.Transform.Name = "Enemy " + count++;
                newEnemy.Logger = Logger;
                newEnemy.Setup(
                    enemy.UnitTemplate.UnitConfig,
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
    }
}
