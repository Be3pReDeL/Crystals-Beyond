using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour
{
    [SerializeField] private TextController _buttonTextController;
    [SerializeField] private Button _buyButton;
    [SerializeField] private int _itemCost;
    [SerializeField] private string _itemName;

    private bool _isPurchased = false;
    private bool _isSelected = false;

    private const float _UPDATINGUIDELAY = 0.05f;

    private void Start()
    {
        LoadState();
        Invoke(nameof(UpdateUI), _UPDATINGUIDELAY);
    }

    private void OnEnable() => Invoke(nameof(UpdateUI), _UPDATINGUIDELAY);

    public void OnBuyButtonClick()
    {
        if (!_isPurchased)
        {
            if (PlayerPointsController.Instance.TrySpendPoints(_itemCost))
            {
                OnPurchased();
            }
        }
        else
        {
            SelectItem();
        }

        ShopManager.Instance.UpdateAllButtonsUI();
    }

    public void OnPurchased()
    {
        _isPurchased = true;
        SavePurchase();
        SelectItem();
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (_isPurchased)
        {
            _buttonTextController.SetText(_isSelected ? "Selected" : "Select");
            _buyButton.interactable = !_isSelected; // Если выбран, кнопка неактивна
        }
        else
        {
            _buttonTextController.SetText(_itemCost.ToString());
            _buyButton.interactable = PlayerPointsController.Instance.GetPoints() >= _itemCost; // Используем новый метод
        }
    }

    public void SelectItem()
    {
        _isSelected = true;
        if (ShopManager.Instance.IsSkin(_itemName))
        {
            ShopManager.Instance.DeselectOtherSkins(_itemName);
        }
        else
        {
            ShopManager.Instance.DeselectOtherBackgrounds(_itemName);
            BackgroundsController.Instance.ApplyBackground(_itemName);
        }
        UpdateUI();
    }

    public void Deselect()
    {
        _isSelected = false;
        UpdateUI();
    }

    private void SavePurchase()
    {
        // Сохранение покупки в PlayerPrefs
        if (ShopManager.Instance.IsSkin(_itemName))
        {
            string purchasedSkins = PlayerPrefsController.GetPurchasedSkins("");
            PlayerPrefsController.SetPurchasedSkins(purchasedSkins + _itemName + ";");
        }
        else
        {
            string purchasedBackgrounds = PlayerPrefsController.GetPurchasedBackgrounds("");
            PlayerPrefsController.SetPurchasedBackgrounds(purchasedBackgrounds + _itemName + ";");
        }
    }

    private void LoadState()
    {
        if (ShopManager.Instance.IsSkin(_itemName))
        {
            string purchasedSkins = PlayerPrefsController.GetPurchasedSkins("");
            _isPurchased = purchasedSkins.Contains(_itemName);
        }
        else
        {
            string purchasedBackgrounds = PlayerPrefsController.GetPurchasedBackgrounds("");
            _isPurchased = purchasedBackgrounds.Contains(_itemName);
        }

        string currentSkin = PlayerPrefsController.GetCurrentSkin("");
        string currentBackground = PlayerPrefsController.GetCurrentBackground("");

        _isSelected = (ShopManager.Instance.IsSkin(_itemName) && currentSkin == _itemName) ||
                     (!ShopManager.Instance.IsSkin(_itemName) && currentBackground == _itemName);
    }

    public string GetItemName()
    {
        return _itemName;
    }
}