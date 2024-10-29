using UnityEngine;

public class SkinsController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerImage;
    [SerializeField] private Sprite[] availableSkins;

    private void Start()
    {
        // Загружаем текущий скин при старте
        ApplySkin(PlayerPrefsController.GetCurrentSkin("Skin 1"));
    }

    public void ApplySkin(string skinName)
    {
        for (int i = 0; i < availableSkins.Length; i++)
        {
            if (availableSkins[i].name == skinName)
            {
                playerImage.sprite = availableSkins[i];
                break;
            }
        }
    }
}
