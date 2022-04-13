using UnityEngine;
using UnityEngine.UI;

public class BoostGauge : MonoBehaviour
{
    [SerializeField]
    private RectTransform inner;

    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Image barImage;

    public float Value
    {
        get => currentValue;
        private set
        {
            currentValue = Mathf.Clamp01(value);
            Vector2 currentAnchorMax = inner.anchorMax;
            currentAnchorMax.x = currentValue;
            inner.anchorMax = currentAnchorMax;
        }
    }
    private float currentValue;

    public void OnBoostAmountChanged(float current, float max)
    {
        Value = current / max;
    }

    public void OnFocusStateChanged(PlayerFocusController.FocusStateInfo focusState)
    {
        backgroundImage.gameObject.SetActive(focusState.current == null);
    }
}
