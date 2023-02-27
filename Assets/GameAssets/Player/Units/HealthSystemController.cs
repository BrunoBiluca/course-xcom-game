using System;
using UnityFoundation.Code;
using UnityFoundation.HealthSystem;

namespace GameAssets
{
    public class HealthSystemController
    {
        private readonly IHealthSystem healthSystem;
        private IHealthBar healthBar;
        private IGameObject diedView;
        private Action diedCallback;

        public HealthSystemController(IHealthSystem healthSystem)
        {
            this.healthSystem = healthSystem;

            healthSystem.OnTakeDamage += Update;
            healthSystem.OnFullyHeal += Update;
            healthSystem.OnDied += DiedHandler;
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

        private void DiedHandler()
        {
            healthBar.Hide();

            if(diedView != null)
            {
                diedView.SetActive(true);
            }

            diedCallback?.Invoke();
        }
    }
}
