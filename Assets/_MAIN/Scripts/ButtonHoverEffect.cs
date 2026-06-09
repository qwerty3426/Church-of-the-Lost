using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    private Vector3 originalScale;

    [Header("Налаштування")]
    public float hoverScale = 1.1f;
    public float speed = 8f;

    private Vector3 targetScale;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.unscaledDeltaTime * speed
        );
    }

    // Курсор НА кнопці
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;
    }

    // Курсор ПІШОВ
    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}
