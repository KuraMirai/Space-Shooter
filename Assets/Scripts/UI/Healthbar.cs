using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] private Slider healthBar;
        [SerializeField] private Image fillImage;
        [SerializeField] private Gradient hpGradient;

        public void Init(float maxHealth)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
            fillImage.color = hpGradient.Evaluate(1);
        }

        public void TakeDamage(float damage)
        {
            healthBar.value -= damage;
            fillImage.color = hpGradient.Evaluate(healthBar.normalizedValue);
        }
    }
}
