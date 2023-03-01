using Assets.UnityFoundation.DamagePopup.Scripts;
using System;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.HealthSystem;

namespace GameAssets
{
    public class HealthSystemController
    {
        private readonly IHealthSystem healthSystem;
        private IHealthBar healthBar;
        private IGameObject diedView;
        private Action diedCallback;
        private ITransform popupTransformRef;

        public HealthSystemController(IHealthSystem healthSystem)
        {
            this.healthSystem = healthSystem;

            healthSystem.OnTakeDamage += Update;
            healthSystem.OnTakeDamageAmount += TakeDamageAmountHandler;
            healthSystem.OnFullyHeal += Update;
            healthSystem.OnDied += DiedHandler;
        }

        public void SetPopupTransform(ITransform popupTransformRef)
        {
            this.popupTransformRef = popupTransformRef;
        }

        public void SetOnDiedCallback(Action diedHandler)
        {
            diedCallback = diedHandler;
        }

        public HealthSystemController AddHealthBar(IHealthBar healthBar)
        {
            this.healthBar = healthBar;
            healthBar.Setup(healthSystem.BaseHealth);
            return this;
        }

        public HealthSystemController AddDiedView(IGameObject diedView)
        {
            this.diedView = diedView;
            diedView.SetActive(false);
            return this;
        }

        private void Update()
        {
            healthBar.SetCurrentHealth(healthSystem.CurrentHealth);
        }

        private void TakeDamageAmountHandler(float amount)
        {
            if(popupTransformRef == null)
                return;

            DamagePopup
                .Create(amount.ToString(), popupTransformRef.Position)
                .SetFontSize(5f);
        }

        private void DiedHandler()
        {
            healthBar.Hide();

            diedView?.SetActive(true);

            diedCallback?.Invoke();
        }
    }
}
