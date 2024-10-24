using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<ShopItemController> skins;
    [SerializeField] private List<ShopItemController> backgrounds;

    private void Start()
    {
        LoadPurchasedItems();
        DeselectOtherSkins(PlayerPrefsController.GetCurrentSkin("Skin 1"));
        DeselectOtherBackgrounds(PlayerPrefsController.GetCurrentSkin("Background 1"));
    }

    private void OnEnable()
    {
        // Каждый раз при активации обновляем состояние покупок
        LoadPurchasedItems();
        DeselectOtherSkins(PlayerPrefsController.GetCurrentSkin("Skin 1"));
        DeselectOtherBackgrounds(PlayerPrefsController.GetCurrentSkin("Background 1"));
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
        PlayerPrefsController.SetCurrentSkin(selectedSkinName);
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
        PlayerPrefsController.SetCurrentBackground(selectedBackgroundName);
    }

    public bool IsSkin(string itemName)
    {
        foreach (var skin in skins)
        {
            if (skin.GetItemName() == itemName) return true;
        }
        return false;
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
