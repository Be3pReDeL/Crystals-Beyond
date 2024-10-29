using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextController : MonoBehaviour, ITextController
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        // Здесь инициализируем компонент TextMeshProUGUI в Start, чтобы избежать ошибок при загрузке UI
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        if (_text == null) _text = GetComponent<TextMeshProUGUI>(); // Подстраховка на случай если _text не успеет проинициализироваться
        _text.text = text;
    }
}
