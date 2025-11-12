using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CenterConstrain : MonoBehaviour
{
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        ApplyCenterConstraint();
    }

    void OnRectTransformDimensionsChange()
    {
        // Reapply the constraint if the screen size changes
        ApplyCenterConstraint();
    }

    private void ApplyCenterConstraint()
    {
        if (rectTransform == null) return;

        // Anchor and pivot in the exact center
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        // Ensure it stays centered
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
