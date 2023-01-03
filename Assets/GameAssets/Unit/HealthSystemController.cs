using UnityFoundation.HealthSystem;
using UnityFoundation.UI.Components;

namespace GameAssets
{
    public class HealthSystemController
    {
        private readonly IHealthSystem healthSystem;
        private IHealthBar healthBar;

        public HealthSystemController(IHealthSystem healthSystem)
        {
            this.healthSystem = healthSystem;

            healthSystem.OnTakeDamage += UpdateHealthBar;
            healthSystem.OnFullyHeal += UpdateHealthBar;
            healthSystem.OnDied += UpdateHealthBar;
        }

        public void AddHealthBar(IHealthBar healthBar)
        {
            this.healthBar = healthBar;
            healthBar.Setup(healthSystem.BaseHealth);
        }

        private void UpdateHealthBar()
        {
            healthBar.SetCurrentHealth(healthSystem.CurrentHealth);
        }
    }
}
