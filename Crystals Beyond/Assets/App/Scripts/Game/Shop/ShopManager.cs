using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [SerializeField] private List<ShopItemController> skins;
    [SerializeField] private List<ShopItemController> backgrounds;

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        LoadPurchasedItems();
        SelectCurrentItems();
    }

    private void SelectCurrentItems()
    {
        string currentSkin = PlayerPrefsController.GetCurrentSkin("Skin 1");
        string currentBackground = PlayerPrefsController.GetCurrentBackground("Background 1");
        
        DeselectOtherSkins(currentSkin);
        DeselectOtherBackgrounds(currentBackground);
        
        // Сохранение текущих выбранных элементов для быстрого доступа
        PlayerPrefsController.SetCurrentSkin(currentSkin);
        PlayerPrefsController.SetCurrentBackground(currentBackground);
    }

    public void DeselectOtherSkins(string selectedSkinName)
    {
        foreach (var skin in skins)
        {
            if (skin.GetItemName() != selectedSkinName)
            {
                skin.Deselect();
            }
        }
    }

    public void UpdateAllButtonsUI() 
    {
        foreach (var skin in skins)
            skin.UpdateUI();

        foreach (var background in backgrounds)
            background.UpdateUI();
    }

    public void DeselectOtherBackgrounds(string selectedBackgroundName)
    {
        foreach (var background in backgrounds)
        {
            if (background.GetItemName() != selectedBackgroundName)
            {
                background.Deselect();
            }
        }
    }

    public bool IsSkin(string itemName)
    {
        // Быстрый поиск по скинам
        return skins.Exists(skin => skin.GetItemName() == itemName);
    }

    private void LoadPurchasedItems()
    {
        // Загружаем все купленные скины и фоны
        string purchasedSkins = PlayerPrefsController.GetPurchasedSkins("");
        string purchasedBackgrounds = PlayerPrefsController.GetPurchasedBackgrounds("");

        foreach (var skin in skins)
        {
            if (purchasedSkins.Contains(skin.GetItemName()))
            {
                skin.OnPurchased();
            }
        }

        foreach (var background in backgrounds)
        {
            if (purchasedBackgrounds.Contains(background.GetItemName()))
            {
                background.OnPurchased();
            }
        }
    }
}
