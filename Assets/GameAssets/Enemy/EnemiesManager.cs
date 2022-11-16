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
        private UnitWorldGridXZManager gridManager;

        private ITurnSystem turnSystem;

        private List<EnemyUnit> enemies;

        public IBilucaLogger Logger { get; set; }

        public void Setup(
            LevelSetupConfig levelSetupConfig,
            UnitWorldGridXZManager gridManager,
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

            EnemyAction(0);
        }

        private void EnemyAction(int index)
        {
            if(index == enemies.Count)
            {
                FinishEnemyTurn();
                return;
            }

            var currentEnemy = enemies[index];

            currentEnemy.OnActionFinished += () => EnemyAction(index + 1);
            currentEnemy.TakeAction();
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
