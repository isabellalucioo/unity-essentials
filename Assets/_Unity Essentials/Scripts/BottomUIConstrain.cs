using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class BottomUIConstrain : MonoBehaviour
{
    [Tooltip("Distance from the bottom of the screen (in pixels).")]
    public float bottomOffset = 0f;

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        ApplyBottomConstraint();
    }

    void OnRectTransformDimensionsChange()
    {
        // Reapply constraint if screen size or resolution changes
        ApplyBottomConstraint();
    }

    private void ApplyBottomConstraint()
    {
        if (rectTransform == null) return;

        // Keep the same horizontal anchors/pivot
        Vector2 currentAnchorMin = rectTransform.anchorMin;
        Vector2 currentAnchorMax = rectTransform.anchorMax;
        Vector2 currentPivot = rectTransform.pivot;

        // Only modify the vertical anchors to stick to bottom
        rectTransform.anchorMin = new Vector2(currentAnchorMin.x, 0f);
        rectTransform.anchorMax = new Vector2(currentAnchorMax.x, 0f);

        // Maintain horizontal position, reset vertical
        Vector2 anchoredPos = rectTransform.anchoredPosition;
        anchoredPos.y = bottomOffset;
        rectTransform.anchoredPosition = anchoredPos;
    }
}
