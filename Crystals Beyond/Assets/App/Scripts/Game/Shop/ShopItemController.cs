using System.Collections;
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

    private const float _UI_UPDATE_DELAY = 0.05f;

    private void Start()
    {
        if (PlayerPointsController.Instance == null || ShopManager.Instance == null)
        {
            Debug.LogError("Не удалось инициализировать ShopItemController: отсутствует один из Singleton-объектов.");
            return;
        }

        LoadState();
        StartCoroutine(DelayedUpdateUI(_UI_UPDATE_DELAY));
    }

    private void OnEnable() => StartCoroutine(DelayedUpdateUI(_UI_UPDATE_DELAY));

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

    private void OnPurchased()
    {
        _isPurchased = true;
        SavePurchase();
        SelectItem();
    }

    public void UpdateUI()
    {
        if (_isPurchased)
        {
            _buttonTextController.SetText(_isSelected ? "Selected" : "Select");
            _buyButton.interactable = !_isSelected;
        }
        else
        {
            _buttonTextController.SetText(_itemCost.ToString());
            _buyButton.interactable = PlayerPointsController.Instance.GetPoints() >= _itemCost;
        }
    }

    public void SelectItem()
    {
        _isSelected = true;

        if (ShopManager.Instance.IsSkin(_itemName))
        {
            ShopManager.Instance.DeselectOtherSkins(_itemName);
            PlayerPrefsController.SetCurrentSkin(_itemName);
        }
        else
        {
            ShopManager.Instance.DeselectOtherBackgrounds(_itemName);
            PlayerPrefsController.SetCurrentBackground(_itemName);
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
            _isPurchased = PlayerPrefsController.GetPurchasedSkins("").Contains(_itemName);
            _isSelected = PlayerPrefsController.GetCurrentSkin("") == _itemName;
        }
        else
        {
            _isPurchased = PlayerPrefsController.GetPurchasedBackgrounds("").Contains(_itemName);
            _isSelected = PlayerPrefsController.GetCurrentBackground("") == _itemName;
        }
    }

    private IEnumerator DelayedUpdateUI(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateUI();
    }

    public string GetItemName() => _itemName;
}
