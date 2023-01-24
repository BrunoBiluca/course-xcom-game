using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.HealthSystem;
using UnityFoundation.ResourceManagement;
using UnityFoundation.SettingsSystem;
using UnityFoundation.UI.Components;

namespace GameAssets
{
    public class PlayerUnit : BilucaMono, ICharacterUnit
    {
        public UnitConfig UnitConfig { get; private set; }

        [SerializeField] private GameObject projectileStart;
        public ITransform ProjectileStart { get; private set; }

        [SerializeField] public GameObject rightShoulderRef;

        public ITransform RightShoulder { get; private set; }

        public IAnimatorController AnimatorController { get; private set; }
        public INavegationAgent TransformNav { get; private set; }

        public string Name => UnitConfig.Name;

        public IHealthSystem HealthSystem { get; private set; }
        public IDamageable Damageable => HealthSystem;

        public IAPActor Actor => unitActionsManager;

        public UnitFactions Faction => UnitFactions.Player;

        public ISelectable Selectable { get; private set; }

        public APActor unitActionsManager;

        private UnitGridWorldCursor worldCursor;
        private SoundEffectsControllerMono soundController;

        protected override void OnAwake()
        {
            TransformNav = new TransformNavegationAgent(Transform) {
                Speed = 10f,
                StoppingDistance = 0.1f
            };

            AnimatorController = GetComponent<UnitAnimatorController>();
            HealthSystem = gameObject.GetComponent<HealthSystemMono>();
            HealthSystem.OnDied += HandleOnDied;

            ProjectileStart = projectileStart.transform.Decorate();
            RightShoulder = rightShoulderRef.transform.Decorate();

            Selectable = new SelectableObject(this);
            Selectable.OnSelectedStateChange += SubscribeExecuteActionEvent;
        }

        public void Start()
        {
            var healthController = new HealthSystemController(HealthSystem);
            healthController.AddHealthBar(transform.FindComponent<IHealthBar>("health_bar"));

            GetComponent<SelectionMarkMono>().Setup(Selectable);

            soundController = GetComponentInChildren<SoundEffectsControllerMono>();
        }

        public void Setup(
            UnitConfig unitConfigTemplate,
            UnitGridWorldCursor worldCursor
        )
        {
            this.worldCursor = worldCursor;

            UnitConfig = unitConfigTemplate;

            HealthSystem.Setup(UnitConfig.InitialHealth);

            unitActionsManager = new APActor(
                new FiniteResourceManager(UnitConfig.MaxActionPoints, true)
            );

            Actor.OnCantExecuteAction += InvokeCantExecuteAction;
            Obj.OnObjectDestroyed += () => Actor.OnCantExecuteAction -= InvokeCantExecuteAction;
        }

        private void HandleOnDied()
        {
            soundController.transform.parent = null;
            soundController.Play();
            soundController.DestroyAfterPlay();
        }

        private void SubscribeExecuteActionEvent()
        {
            worldCursor.OnAvaiableCellSecondaryClicked -= ExecuteAction;
            if(Selectable.IsSelected)
                worldCursor.OnAvaiableCellSecondaryClicked += ExecuteAction;
        }

        private void ExecuteAction() => Actor.Execute();

        private void InvokeCantExecuteAction() => Actor.UnsetAction();
    }
}