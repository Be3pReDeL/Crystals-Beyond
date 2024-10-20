using UnityEngine;
using DG.Tweening;

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

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        
        // Сохраняем начальное положение элемента
        _initialPosition = _rectTransform.anchoredPosition;
        _canvasGroup.alpha = 0f;  // По умолчанию элемент невидим
    }

    public void Appear()
    {
        gameObject.SetActive(true);  // Активируем элемент
        Vector2 startPosition = GetStartPosition();

        // Начальное состояние (вне экрана)
        _rectTransform.anchoredPosition = startPosition;
        _canvasGroup.alpha = 0f;

        // Анимация вылета + увеличение прозрачности
        _rectTransform.DOAnchorPos(_initialPosition, _animationDuration).SetEase(Ease.OutCubic);
        _canvasGroup.DOFade(1f, _fadeInDuration).SetEase(Ease.OutCubic);
    }

    public void Disappear()
    {
        Vector2 targetPosition = GetStartPosition();

        // Анимация удаления (вылет + уменьшение прозрачности)
        _rectTransform.DOAnchorPos(targetPosition, _animationDuration).SetEase(Ease.InCubic);
        _canvasGroup.DOFade(0f, _fadeOutDuration).SetEase(Ease.InCubic)
            .OnComplete(() => gameObject.SetActive(false));  // Отключаем элемент после завершения анимации
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
