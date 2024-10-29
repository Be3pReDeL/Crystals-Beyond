using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthUIController : MonoBehaviour
{
    private Slider _healthSlider;  // Слайдер для отображения здоровья

    private void Awake() => _healthSlider = GetComponent<Slider>();

    public void InitializeHealthUI(int maxHealth, int currentHealth)
    {
        // Инициализация слайдера
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = currentHealth;
    }

    public void UpdateHealthUI(int currentHealth)
    {
        // Обновление значения на слайдере
        _healthSlider.value = currentHealth;
    }
}
