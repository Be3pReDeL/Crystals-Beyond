using UnityEngine;

public class SkinsController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerImage;
    [SerializeField] private Sprite[] availableSkins;
    private int _currentSkinIndex;

    private void Start()
    {
        // Проверяем наличие скинов перед началом
        if (availableSkins == null || availableSkins.Length == 0)
        {
            Debug.LogError("SkinsController: No skins available.");
            return;
        }
        
        // Загружаем и применяем текущий скин при старте
        var skinName = PlayerPrefsController.GetCurrentSkin("Skin 1");
        ApplySkin(skinName);
    }

    public void ApplySkin(string skinName)
    {
        _currentSkinIndex = FindSkinIndexByName(skinName);
        
        if (_currentSkinIndex != -1)
        {
            playerImage.sprite = availableSkins[_currentSkinIndex];
        }
        else
        {
            Debug.LogWarning($"Skin '{skinName}' not found. Applying default skin.");
            playerImage.sprite = availableSkins[0];  // Применение скина по умолчанию
        }
    }

    private int FindSkinIndexByName(string skinName)
    {
        for (int i = 0; i < availableSkins.Length; i++)
        {
            if (availableSkins[i].name == skinName)
            {
                return i;
            }
        }
        return -1;  // Возвращаем -1, если скин не найден
    }
}
