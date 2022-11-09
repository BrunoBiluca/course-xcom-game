using UnityEngine;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.Grid;
using UnityFoundation.Tools.TimeUtils;

namespace GameAssets
{
    public class EnemiesManager : MonoBehaviour, IBilucaLoggable
    {
        private LevelSetupConfig levelSetupConfig;
        private UnitWorldGridXZManager gridManager;

        private ITimer timer;
        private ITurnSystem turnSystem;

        public IBilucaLogger Logger { get; set; }

        public void Setup(
            LevelSetupConfig levelSetupConfig,
            UnitWorldGridXZManager gridManager,
            ITurnSystem turnSystem
        )
        {
            this.levelSetupConfig = levelSetupConfig;
            this.gridManager = gridManager;

            timer = new Timer(2f, FinishEnemyTurn).RunOnce();

            this.turnSystem = turnSystem;
            turnSystem.OnEnemyTurnStarted += EnemyStartTurn;
            SetupUnits();
        }

        private void EnemyStartTurn()
        {
            timer.Start();
            Logger?.Log("Start enemy turn");
        }

        private void FinishEnemyTurn()
        {
            Logger?.Log("Finish enemy turn");
            timer.Stop();
            turnSystem.EndEnemyTurn();
        }

        public void SetupUnits()
        {
            foreach(var enemy in levelSetupConfig.Enemies)
            {
                var newEnemy = Instantiate(enemy.EnemyPrefab).GetComponent<EnemyUnit>();

                newEnemy.Transform.Position = gridManager.Grid
                    .GetCellCenterPosition(
                        new GridCellPositionXZ(enemy.Position.X, enemy.Position.Z)
                );

                gridManager.Add(newEnemy);
            }
        }
    }
}
