using UnityEngine;
using UnityEngine.EventSystems;

public class UIMainMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 originalScale = new Vector3(2f, 2f, 1f);
    public Vector3 hoverScale = new Vector3(2.2f, 2.2f, 1f);

    public void Start()
    {
        originalScale = transform.localScale;
        hoverScale.x = originalScale.x + 0.2f;
        hoverScale.y = originalScale.y + 0.2f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
