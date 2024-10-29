using UnityEngine;
using CandyCoded.HapticFeedback;

public class VibrationController : MonoBehaviour
{
    public static VibrationController Instance { get; private set;}

    public enum VibrationType { light, medium, heavy }

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    public void SetVibrationEnabled(bool isEnabled)
    {
        // Вибрация не связана с микшером, здесь мы просто сохраняем настройку
        PlayerPrefsController.SetVibrationEnabled(isEnabled);

        // Если вибрация включена, можно, например, сразу вызвать вибрацию для проверки
        if (isEnabled)
            Vibrate(VibrationType.light);
    }

    public void Vibrate(VibrationType vibrationType) 
    {
        if(PlayerPrefsController.IsVibrationEnabled())
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
                    goto case VibrationType.light;
            }
        }
    }
}
