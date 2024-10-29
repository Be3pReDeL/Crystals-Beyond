using UnityEngine;
using CandyCoded.HapticFeedback;

public class VibrationController : MonoBehaviour
{
    public static VibrationController Instance { get; private set; }

    public enum VibrationType { light, medium, heavy }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Сохраняем вибрацию между сценами, если нужно
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVibrationEnabled(bool isEnabled)
    {
        PlayerPrefsController.SetVibrationEnabled(isEnabled);

        // Проверка доступности вибрации
        if (isEnabled && SystemInfo.supportsVibration)
        {
            Vibrate(VibrationType.light);
        }
    }

    public void Vibrate(VibrationType vibrationType)
    {
        if (PlayerPrefsController.IsVibrationEnabled() && SystemInfo.supportsVibration)
        {
            switch (vibrationType)
            {
                case VibrationType.light:
                    HapticFeedback.LightFeedback();
                    break;
                
                case VibrationType.medium:
                    HapticFeedback.MediumFeedback();
                    break;
                
                case VibrationType.heavy:
                    HapticFeedback.HeavyFeedback();
                    break;

                default:
                    HapticFeedback.LightFeedback();
                    break;
            }
        }
    }
}
