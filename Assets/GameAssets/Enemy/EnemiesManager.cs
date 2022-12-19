using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.Grid;
using UnityFoundation.TurnSystem;

namespace GameAssets
{
    public class EnemiesManager : MonoBehaviour, IBilucaLoggable
    {
        private LevelSetupConfig levelSetupConfig;
        private UnitWorldGridManager gridManager;
        private ITurnSystem turnSystem;

        private List<EnemyUnit> enemies;

        public IBilucaLogger Logger { get; set; }

        private int enemyIndex;

        public void Setup(
            LevelSetupConfig levelSetupConfig,
            UnitWorldGridManager gridManager,
            ITurnSystem turnSystem
        )
        {
            this.levelSetupConfig = levelSetupConfig;
            this.gridManager = gridManager;

            this.turnSystem = turnSystem;
            turnSystem.OnEnemyTurnStarted += EnemyStartTurn;
            SetupUnits();
        }

        private void EnemyStartTurn()
        {
            Logger?.Log("Start enemy turn");

            enemies.RemoveAll(e => e == null);

            enemyIndex = 0;
            EnemyAction();
        }

        private void EnemyAction()
        {
            var currentEnemy = enemies[enemyIndex];
            currentEnemy.OnActionFinished -= NextEnemy;
            currentEnemy.OnActionFinished += NextEnemy;
            currentEnemy.TakeActions();
        }

        private void NextEnemy()
        {
            enemyIndex++;
            if(enemyIndex >= enemies.Count)
            {
                FinishEnemyTurn();
                return;
            }

            EnemyAction();
        }

        private void FinishEnemyTurn()
        {
            Logger?.Log("Finish enemy turn");
            turnSystem.EndEnemyTurn();
        }

        public void SetupUnits()
        {
            enemies = new List<EnemyUnit>();
            foreach(var enemy in levelSetupConfig.Enemies)
            {
                var newEnemy = Instantiate(enemy.EnemyPrefab).GetComponent<EnemyUnit>();
                newEnemy.Setup(enemy.UnitTemplate);

                newEnemy.Transform.Position = gridManager.Grid
                    .GetCellCenterPosition(
                        new GridCellPositionXZ(enemy.Position.X, enemy.Position.Z)
                );

                enemies.Add(newEnemy);
                gridManager.Add(newEnemy);
            }
        }
    }
}
