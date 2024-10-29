using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BackgroundsController : MonoBehaviour
{
    public static BackgroundsController Instance { get; private set; }

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite[] availableBackgrounds;

    private Dictionary<string, Sprite> _backgroundDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeBackgroundDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        string backgroundName = PlayerPrefsController.GetCurrentBackground("Background 1") ?? "Background 1";
        ApplyBackground(backgroundName);
    }

    private void InitializeBackgroundDictionary()
    {
        _backgroundDictionary = new Dictionary<string, Sprite>();
        foreach (Sprite sprite in availableBackgrounds)
        {
            _backgroundDictionary[sprite.name] = sprite;
        }
    }

    public void ApplyBackground(string backgroundName)
    {
        if (_backgroundDictionary.TryGetValue(backgroundName, out Sprite backgroundSprite))
        {
            backgroundImage.sprite = backgroundSprite;
        }
        else
        {
            Debug.LogWarning($"Background '{backgroundName}' not found. Ensure the background name is correct.");
        }
    }
}
