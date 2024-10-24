using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour
{
    [SerializeField] private PlayerPointsController pointsController;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private TextController buttonTextController;
    [SerializeField] private Button buyButton;
    [SerializeField] private int itemCost;
    [SerializeField] private string itemName;  // Название товара (скин или фон)

    private bool isPurchased = false;
    private bool isSelected = false;

    private void Start()
    {
        // Начальная установка состояния кнопки
        LoadState();
        UpdateUI();
    }

    private void OnEnable()
    {
        // Каждый раз при активации объекта загружаем его состояние
        LoadState();
        UpdateUI();
    }

    public void OnBuyButtonClick()
    {
        if (!isPurchased)
        {
            if (pointsController.TrySpendPoints(itemCost))
            {
                OnPurchased();
            }
        }
        else
        {
            SelectItem();
        }
    }

    public void OnPurchased()
    {
        isPurchased = true;
        SavePurchase();
        SelectItem();
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (isPurchased)
        {
            if (isSelected)
            {
                buttonTextController.SetText("Selected");
                buyButton.interactable = false;
            }
            else
            {
                buttonTextController.SetText("Select");
                buyButton.interactable = true;
            }
        }
        else
        {
            buttonTextController.SetText(itemCost.ToString());
            buyButton.interactable = PlayerPrefsController.GetPoints(0) >= itemCost;
        }
    }

    public void SelectItem()
    {
        isSelected = true;
        if (shopManager.IsSkin(itemName))
        {
            shopManager.DeselectOtherSkins(itemName);  // Деселект остальных скинов
        }
        else
        {
            shopManager.DeselectOtherBackgrounds(itemName);  // Деселект остальных фонов
        }
        UpdateUI();
    }

    public void Deselect()
    {
        isSelected = false;
        UpdateUI();
    }

    private void SavePurchase()
    {
        // Сохранение покупки в PlayerPrefs
        if (shopManager.IsSkin(itemName))
        {
            string purchasedSkins = PlayerPrefsController.GetPurchasedSkins("");
            PlayerPrefsController.SetPurchasedSkins(purchasedSkins + itemName + ";");
        }
        else
        {
            string purchasedBackgrounds = PlayerPrefsController.GetPurchasedBackgrounds("");
            PlayerPrefsController.SetPurchasedBackgrounds(purchasedBackgrounds + itemName + ";");
        }
    }

    private void LoadState()
    {
        // Загружаем состояние покупки и выбранности
        if (shopManager.IsSkin(itemName))
        {
            string purchasedSkins = PlayerPrefsController.GetPurchasedSkins("");
            isPurchased = purchasedSkins.Contains(itemName);
        }
        else
        {
            string purchasedBackgrounds = PlayerPrefsController.GetPurchasedBackgrounds("");
            isPurchased = purchasedBackgrounds.Contains(itemName);
        }

        // Загружаем текущее состояние выбранного скина или фона
        string currentSkin = PlayerPrefsController.GetCurrentSkin("");
        string currentBackground = PlayerPrefsController.GetCurrentBackground("");

        if (shopManager.IsSkin(itemName))
        {
            isSelected = currentSkin == itemName;
        }
        else
        {
            isSelected = currentBackground == itemName;
        }
    }

    public string GetItemName()
    {
        return itemName;
    }
}
