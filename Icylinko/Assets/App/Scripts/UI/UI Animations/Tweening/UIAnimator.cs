using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("UI/Animations/UI Animator")]
public class UIAnimator : MonoBehaviour
{
    public enum SlideDirection { LeftToRight, RightToLeft, BottomToTop, TopToBottom }

    // Настраиваемые параметры
    [SerializeField] private float _animationDuration = 0.5f;  // Длительность анимации
    [SerializeField] private SlideDirection _slideDirection = SlideDirection.LeftToRight;  // Направление вылета
    [SerializeField] private float _fadeInDuration = 0.3f;     // Длительность анимации появления
    [SerializeField] private float _fadeOutDuration = 0.3f;    // Длительность анимации исчезновения
    [SerializeField] private float _slideOffset = 500f;        // Дистанция смещения при вылете

    // UI элементы для анимации
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;

    private Vector2 _initialPosition;

    private const float _DEACTIVATESCREENDELAY = 0.01f;

    private bool _isAnimating; // Флаг анимации

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();

        // Сохраняем начальное положение элемента
        _initialPosition = _rectTransform.anchoredPosition;
        _canvasGroup.alpha = 0f;  // По умолчанию элемент невидим
    }

    private void OnEnable() => Appear();

    public void Appear()
    {
        if (TryGetComponent(out Button button))
            button.interactable = true;

        gameObject.SetActive(true);  // Активируем элемент
        Vector2 startPosition = GetStartPosition();

        // Начальное состояние (вне экрана)
        _rectTransform.anchoredPosition = startPosition;
        _canvasGroup.alpha = 0f;

        // Анимация вылета + увеличение прозрачности
        _isAnimating = true; // Устанавливаем флаг анимации
        _rectTransform.DOAnchorPos(_initialPosition, _animationDuration).SetEase(Ease.OutCubic).OnComplete(() => _isAnimating = false);
        _canvasGroup.DOFade(1f, _fadeInDuration).SetEase(Ease.OutCubic);
    }

    public void Disappear(GameObject gameObject)
    {
        if (_isAnimating) // Проверяем, идет ли анимация
        {
            Debug.Log("Animation in progress, cannot disappear yet.");
            return; // Если анимация еще не завершена, игнорируем вызов
        }

        // Анимация удаления (вылет + уменьшение прозрачности)
        Vector2 targetPosition = GetStartPosition();
        
        if (TryGetComponent(out Button button))
            button.interactable = false;

        _isAnimating = true; // Устанавливаем флаг анимации
        _rectTransform.DOAnchorPos(targetPosition, _animationDuration).SetEase(Ease.InCubic);
        _canvasGroup.DOFade(0f, _fadeOutDuration).SetEase(Ease.InCubic)
            .OnComplete(() => StartCoroutine(DisableGameobjectCoroutine(_DEACTIVATESCREENDELAY, gameObject)));
    }

    private IEnumerator DisableGameobjectCoroutine(float delay, GameObject gameObject) 
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
        _isAnimating = false; // Сбрасываем флаг анимации
    }

    private Vector2 GetStartPosition()
    {
        Vector2 startPosition = _initialPosition;

        switch (_slideDirection)
        {
            case SlideDirection.LeftToRight:
                startPosition.x -= _slideOffset;
                break;
            case SlideDirection.RightToLeft:
                startPosition.x += _slideOffset;
                break;
            case SlideDirection.BottomToTop:
                startPosition.y -= _slideOffset;
                break;
            case SlideDirection.TopToBottom:
                startPosition.y += _slideOffset;
                break;
        }

        return startPosition;
    }
}
