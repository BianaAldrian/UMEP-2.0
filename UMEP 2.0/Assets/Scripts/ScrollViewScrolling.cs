using UnityEngine;
using UnityEngine.UI;

public class ScrollViewScrolling : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Scrollbar scrollbar;

    // Set this speed to control the scrolling speed.
    public float scrollSpeed = 1.0f;

    private void Start()
    {
        // Attach the Scrollbar to the ScrollRect.
        scrollbar.onValueChanged.AddListener(ScrollScrollRect);
    }

    private void ScrollScrollRect(float value)
    {
        // Calculate the desired vertical position within the ScrollRect content.
        float targetPosition = value * (scrollRect.content.rect.height - scrollRect.viewport.rect.height);

        // Create a new position with the same x value but the target y value.
        Vector2 newPosition = new Vector2(scrollRect.content.anchoredPosition.x, -targetPosition);

        // Lerp smoothly to the new position.
        scrollRect.content.anchoredPosition = Vector2.Lerp(scrollRect.content.anchoredPosition, newPosition, scrollSpeed * Time.deltaTime);
    }
}
