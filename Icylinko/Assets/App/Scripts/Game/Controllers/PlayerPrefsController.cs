using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{
    private const string GameModeKey = "Game Mode";
    private const string LevelKey = "Level";
    private const string CompletedLevelsKey = "Completed Levels";
    private const string CurrentSkinKey = "Current Skin";
    private const string PurchasedSkinsKey = "Purchased Skins";
    private const string CurrentBackgroundKey = "Current Background";
    private const string PurchasedBackgroundsKey = "Purchased Backgrounds";
    private const string PointsKey = "Points";
    private const string LevelUnlockedKeyPrefix = "LevelUnlocked_"; // Префикс для ключей уровней
    public const string MusicPrefKey = "MusicEnabled";
    public const string SoundPrefKey = "SoundEnabled";
    public const string VibrationPrefKey = "VibrationEnabled";

    private void Awake() 
    {
        SetLevelUnlocked(0, true); // Открываем нулевой уровень
    }

    // Game Mode
    public static void SetGameMode(GameController.GameMode mode)
    {
        PlayerPrefs.SetInt(GameModeKey, (int)mode);
    }

    public static GameController.GameMode GetGameMode(GameController.GameMode defaultMode)
    {
        return (GameController.GameMode)PlayerPrefs.GetInt(GameModeKey, (int)defaultMode);
    }

    // Level
    public static void SetLevel(int level)
    {
        PlayerPrefs.SetInt(LevelKey, level);
    }

    public static int GetLevel(int defaultLevel)
    {
        return PlayerPrefs.GetInt(LevelKey, defaultLevel);
    }

    // Completed Levels
    public static void SetCompletedLevels(int completedLevels)
    {
        PlayerPrefs.SetInt(CompletedLevelsKey, completedLevels);
    }

    public static int GetCompletedLevels(int defaultLevels)
    {
        return PlayerPrefs.GetInt(CompletedLevelsKey, defaultLevels);
    }

    // Level Unlocked
    public static void SetLevelUnlocked(int level, bool isUnlocked)
    {
        PlayerPrefs.SetInt(LevelUnlockedKeyPrefix + level, isUnlocked ? 1 : 0);
    }

    public static bool IsLevelUnlocked(int level)
    {
        return PlayerPrefs.GetInt(LevelUnlockedKeyPrefix + level, 0) == 1;
    }

    // Current Skin
    public static void SetCurrentSkin(string skin)
    {
        PlayerPrefs.SetString(CurrentSkinKey, skin);
    }

    public static string GetCurrentSkin(string defaultSkin)
    {
        return PlayerPrefs.GetString(CurrentSkinKey, defaultSkin);
    }

    // Purchased Skins
    public static void SetPurchasedSkins(string skins)
    {
        PlayerPrefs.SetString(PurchasedSkinsKey, skins);
    }

    public static string GetPurchasedSkins(string defaultSkins)
    {
        return PlayerPrefs.GetString(PurchasedSkinsKey, defaultSkins);
    }

    // Current Background
    public static void SetCurrentBackground(string background)
    {
        PlayerPrefs.SetString(CurrentBackgroundKey, background);
    }

    public static string GetCurrentBackground(string defaultBackground)
    {
        return PlayerPrefs.GetString(CurrentBackgroundKey, defaultBackground);
    }

    // Purchased Backgrounds
    public static void SetPurchasedBackgrounds(string backgrounds)
    {
        PlayerPrefs.SetString(PurchasedBackgroundsKey, backgrounds);
    }

    public static string GetPurchasedBackgrounds(string defaultBackgrounds)
    {
        return PlayerPrefs.GetString(PurchasedBackgroundsKey, defaultBackgrounds);
    }

    // Points
    public static void SetPoints(int points)
    {
        PlayerPrefs.SetInt(PointsKey, points < 0 ? 0 : points);
    }

    public static int GetPoints(int defaultPoints)
    {
        return PlayerPrefs.GetInt(PointsKey, defaultPoints);
    }

    // Музыка
    public static void SetMusicEnabled(bool enabled)
    {
        SetBool(MusicPrefKey, enabled);
    }

    public static bool IsMusicEnabled()
    {
        return GetBool(MusicPrefKey, true);
    }

    // Звук
    public static void SetSoundEnabled(bool enabled)
    {
        SetBool(SoundPrefKey, enabled);
    }

    public static bool IsSoundEnabled()
    {
        return GetBool(SoundPrefKey, true);
    }

    // Вибрация
    public static void SetVibrationEnabled(bool enabled)
    {
        SetBool(VibrationPrefKey, enabled);
    }

    public static bool IsVibrationEnabled()
    {
        return GetBool(VibrationPrefKey, true);
    }

    // Универсальные методы для работы с bool
    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public static bool GetBool(string key, bool defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
    }
}
