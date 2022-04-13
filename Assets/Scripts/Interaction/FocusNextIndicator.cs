using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FocusNextIndicator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Image arrow;

    [SerializeField]
    private TMP_Text label;

    [SerializeField]
    private TMP_Text displayName;

    [SerializeField]
    private TMP_Text key;

    [Header("Settings")]
    [SerializeField]
    private Color activeColor = Color.white;

    [SerializeField]
    private Color inactiveColor = Color.gray;

    private string defaultLabel;

    private void Awake()
    {
        // Save the original text
        defaultLabel = label.text;

        ShowState(null);
    }

    public void ShowState(Focusable focusable)
    {
        bool isActive = focusable != null;

        Color color = isActive ? activeColor : inactiveColor;
        arrow.color = color;
        label.color = color;
        displayName.color = color;
        key.color = color;

        label.text = isActive ? defaultLabel : "Exit";
        displayName.text = isActive ? focusable.Data.DisplayName : "";
    }
}
