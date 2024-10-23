using UnityEngine;

public class VibrationController : MonoBehaviour
{
    public static VibrationController Instance { get; private set;}

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
        {
            Handheld.Vibrate();
        }
    }

    public void Vibrate() => Handheld.Vibrate();
}
