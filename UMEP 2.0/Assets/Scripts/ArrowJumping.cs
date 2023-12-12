using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowJumping : MonoBehaviour
{
    public Transform targetPosition; // Set this in the Inspector to the target position
    public float jumpSpeed = 5f;

    private ScrollRect scrollRect;
    private RectTransform arrowRectTransform;
    private bool isJumping = true; // Set to true to start jumping

    void Start()
    {
        // Try to find the ScrollRect component in parent
        scrollRect = GetComponentInParent<ScrollRect>();

        // Try to find the RectTransform component on the arrow GameObject
        arrowRectTransform = GetComponent<RectTransform>();

        // Check if either of the components is not found
        if (scrollRect == null || arrowRectTransform == null)
        {
            Debug.LogError("ScrollRect or RectTransform not found. Make sure the script is attached to the correct GameObject.");
            return;
        }
    }

    void Update()
    {
        if (isJumping)
        {
            JumpToTarget();
        }
    }

    void JumpToTarget()
    {
        // Make sure scrollRect and arrowRectTransform are not null before proceeding
        if (scrollRect == null || arrowRectTransform == null)
        {
            return;
        }

        // Calculate the direction to the target
        Vector2 direction = (targetPosition.position - arrowRectTransform.position).normalized;

        // Move the arrow towards the target
        arrowRectTransform.anchoredPosition += direction * jumpSpeed * Time.deltaTime;

        // Check if the arrow is close enough to the target
        float distance = Vector2.Distance(arrowRectTransform.position, targetPosition.position);
        if (distance < 0.1f)
        {
            isJumping = false;
        }

        // Ensure the arrow stays within the scroll view content
        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(arrowRectTransform.anchoredPosition.x, 0, scrollRect.content.rect.width),
            Mathf.Clamp(arrowRectTransform.anchoredPosition.y, 0, scrollRect.content.rect.height)
        );

        arrowRectTransform.anchoredPosition = clampedPosition;
    }
}
