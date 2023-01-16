using System.Threading.Tasks;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.HealthSystem;
using UnityFoundation.Physics3D;
using UnityFoundation.ResourceManagement;
using UnityFoundation.UI.Components;

namespace GameAssets
{
    public class EnemyUnit : BilucaMono, IAIUnit
    {
        [SerializeField] private GameObject ragdoll;

        [SerializeField] private Transform root;
        [SerializeField] private GameObject projectileStart;

        public string Name => Transform.Name;

        public IHealthSystem HealthSystem { get; private set; }

        public IDamageable Damageable => HealthSystem;

        public IAPActor Actor { get; private set; }

        public UnitConfig UnitConfig { get; private set; }

        public ITransform RightShoulder { get; private set; }

        public IAnimatorController AnimatorController { get; private set; }

        public ITransform ProjectileStart { get; private set; }

        public INavegationAgent TransformNav { get; private set; }

        public UnitFactions Faction => UnitFactions.Enemy;

        private EnemyBrain brain;

        public void Setup(
            UnitConfig unitConfigTemplate,
            UnitWorldGridManager gridManager,
            IEnemyActionIntentFactory intentFactory
        )
        {
            UnitConfig = unitConfigTemplate;

            brain = new EnemyBrain(this, intentFactory, gridManager) { Logger = Logger };

            OnSetup();
        }

        private void OnSetup()
        {
            Actor = new APActor(new FiniteResourceManager(UnitConfig.MaxActionPoints, true));

            ProjectileStart = projectileStart.transform.Decorate();

            AnimatorController = GetComponent<UnitAnimatorController>();

            HealthSystem = GetComponent<HealthSystemMono>();
            HealthSystem.Setup(UnitConfig.InitialHealth);
            HealthSystem.OnDied += DieHandler;

            var healthController = new HealthSystemController(HealthSystem);
            healthController.AddHealthBar(transform.FindComponent<IHealthBar>("health_bar"));

            TransformNav = new TransformNavegationAgent(Transform) {
                Speed = 10f,
                StoppingDistance = 0.1f
            };
        }

        private void DieHandler()
        {
            var ragdollHandler = Instantiate(ragdoll, transform.position, transform.rotation)
                .GetComponent<RagdollHandler>();

            ragdollHandler.Setup(root.Decorate());
        }

        private bool canTakeNextDecision;

        public async Task TakeActions()
        {
            Actor.OnActionFinished += EnabledNextDecision;

            Logger?.LogHighlight(Name, "Start taking actions");

            canTakeNextDecision = true;

            await LoopConditionAsync.While(CanAct).Loop(DecideAction);

            Logger?.LogHighlight(Name, "finished take actions");
            Actor.OnActionFinished -= EnabledNextDecision;
        }

        private void DecideAction()
        {
            if(!canTakeNextDecision)
                return;

            canTakeNextDecision = false;
            var chosenIntent = brain.ChooseIntent();

            chosenIntent.Some(i => {
                Logger?.LogHighlight(Name, "has the intent of", i.GetType().ToString());
                Actor.Set(i);
            }).OrElse(() => {
                Actor.ActionPoints.Emptify();
            });
        }

        private void EnabledNextDecision()
        {
            canTakeNextDecision = true;
        }

        private bool CanAct()
        {
            return Actor.ActionPoints.CurrentAmount > 0;
        }
    }
}
