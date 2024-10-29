using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class IcePowerUIController : MonoBehaviour
{
    private Slider _icePowerSlider;  // Слайдер для шкалы Ice Power

    private void Awake() => _icePowerSlider = GetComponent<Slider>();

    // Инициализация с заданными максимальным значением и текущим значением
    public void InitializeIcePowerUI(float maxValue, float currentValue)
    {
        _icePowerSlider.maxValue = maxValue;
        _icePowerSlider.value = currentValue;
    }

    // Обновление текущего значения слайдера
    public void UpdateIcePowerUI(float currentValue)
    {
        _icePowerSlider.value = currentValue;
    }
}
