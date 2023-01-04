using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.TurnSystem;

namespace GameAssets
{
    public class GameInstaller : Singleton<GameInstaller>, IPrettyable
    {
        public event Action OnInstallerFinish;

        [Header("Config")]
        [SerializeField] private LevelSetupConfig levelSetupConfig;

        [Header("UI")]
        [SerializeField] private APUnitActionSelectionView unitActionSelectionView;
        [SerializeField] private ActionPointsView actionPointsView;
        [SerializeField] private TurnSystemView turnSystemView;
        [SerializeField] private WorldGridView worldGridView;
        [SerializeField] private UnitsView unitsView;

        [Header("Grid")]
        [SerializeField] private UnitGridWorldCursor worldCursor;

        [Header("Managers")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private UnitSelectionMono unitSelection;
        [SerializeField] private UnitsManager unitsManager;
        [SerializeField] private EnemiesManager enemiesManager;
        [SerializeField] private ProjectileFactory projectileFactory;
        [SerializeField] private GrenadeProjectileFactory grenadeFactory;

        [SerializeField] private GameSceneManager sceneManager;
        [SerializeField] private GameBinder binder;

        public UnitWorldGridManager GridManager { get; private set; }

        protected override void OnAwake()
        {
            binder.OnBinderFinish += StartInstaller;
        }

        private void StartInstaller()
        {
            Debug.Log("Start GameInstaller");
            var grid = binder.GetReference<UnitWorldGridXZ>();
            GridManager = new UnitWorldGridManager(grid.Grid);

            var raycastHandler = new RaycastHandler(new CameraDecorator(Camera.main));
            worldCursor.Setup(raycastHandler, grid.Grid, GridManager);

            worldGridView.Setup(
                new WorldGridView.Params() {
                    gridManager = GridManager,
                    worldCursor = worldCursor
                }
            );

            var unitSelection = FindObjectOfType<UnitSelectionMono>();
            unitSelection.Setup(worldCursor);

            unitSelection.OnUnitUnselected += () => GridManager.ResetValidation();

            var unitActionsFactory = new UnitActionsFactory(
                unitSelection,
                worldCursor,
                GridManager,
                projectileFactory,
                grenadeFactory,
                levelSetupConfig.actionPointsConfig
            );

            var actionSelection = new APUnitActionSelection(unitSelection);

            actionSelection.OnActionUnselected += () => GridManager.ResetValidation();

            unitActionSelectionView.Setup(actionSelection, unitActionsFactory);

            actionPointsView.Setup(unitSelection);

            var selectableVisibility = new SelectableVisibilityHandler(
                new GameObjectDecorator(actionPointsView.gameObject),
                new GameObjectDecorator(unitActionSelectionView.gameObject)
            ) {
                Logger = binder.GetReference<IBilucaLogger>()
            };
            selectableVisibility.Hide();

            unitSelection.OnUnitSelected += () => selectableVisibility.Show();
            unitSelection.OnUnitUnselected += () => selectableVisibility.Hide();

            var turnSystem = new TurnSystem();

            unitsManager.Setup(
                levelSetupConfig, worldCursor, GridManager, turnSystem, unitSelection
            );

            enemiesManager.Logger = binder.GetReference<IBilucaLogger>();
            enemiesManager.Setup(levelSetupConfig, GridManager, turnSystem);

            turnSystemView.Setup(turnSystem);

            foreach(var unit in FindObjectsOfType<MonoBehaviour>().OfType<IUnit>())
            {
                GridManager.Add(unit);
            }

            gameManager.Setup(unitsManager, enemiesManager);

            unitsView.Setup(unitsManager);

            OnInstallerFinish?.Invoke();
            Debug.Log("Finish GameInstaller");
        }

        public void Update()
        {
            if(GridManager == null) return;

            GridManager.Update();
        }

        public PrettyObject BePretty()
        {
            var installerColor = new Color(.38f, .35f, .06f);
            return new PrettyObject(false, installerColor, Color.white, gameObject);
        }
    }
}
