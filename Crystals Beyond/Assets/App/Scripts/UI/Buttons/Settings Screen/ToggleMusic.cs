public class ToggleMusic : ToggleSetting
{
    protected override string PlayerPrefKey => PlayerPrefsController.MusicPrefKey;

    protected override void ApplySetting()
    {
        SoundController.Instance.SetMusicEnabled(_isEnabled);
    }
}
