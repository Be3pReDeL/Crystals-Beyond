public class ToggleSound : ToggleSetting
{
    protected override string PlayerPrefKey => PlayerPrefsController.SoundPrefKey;

    protected override void ApplySetting()
    {
        SoundController.Instance.SetSoundEnabled(_isEnabled);
    }
}
