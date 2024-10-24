using UnityEngine;

public class SkinsController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerImage;
    [SerializeField] private Sprite[] availableSkins;

    private void Start()
    {
        // Загружаем текущий скин при старте
        LoadCurrentSkin();
    }

    private void OnEnable()
    {
        // Загружаем скин каждый раз, когда объект активируется
        LoadCurrentSkin();
    }

    public void ApplySkin(string skinName)
    {
        for (int i = 0; i < availableSkins.Length; i++)
        {
            if (availableSkins[i].name == skinName)
            {
                playerImage.sprite = availableSkins[i];
                PlayerPrefsController.SetCurrentSkin(skinName);
                break;
            }
        }
    }

    private void LoadCurrentSkin()
    {
        string savedSkin = PlayerPrefsController.GetCurrentSkin("Skin 1");
        ApplySkin(savedSkin);
    }
}
