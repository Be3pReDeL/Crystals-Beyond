public class ToggleVibration : ToggleSetting
{
    protected override string PlayerPrefKey => PlayerPrefsController.VibrationPrefKey;

    protected override void ApplySetting()
    {
        VibrationController.Instance.SetVibrationEnabled(_isEnabled);
    }
}
