using Cinemachine;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(FocusDetection))]
public class Focusable : MonoBehaviour
{
    [Header("Zoom-in Focus Control")]
    [SerializeField]
    private CinemachineVirtualCamera focusCamera;
    [SerializeField]
    private int focusPriority = 20;
    [SerializeField]
    private int unfocusPriority = 0;

    [Header("Data Display")]
    [SerializeField]
    private BodyData data;
    [SerializeField]
    private DetailsPanel detailsPanel;

    [Header("Debug")]
    [SerializeField]
    private bool canBeFocused;

    public CinemachineVirtualCamera FocusCamera => focusCamera;

    private FocusDetection detection;

    private void Awake()
    {
        detection = GetComponent<FocusDetection>();
    }

    private void Start()
    {
        focusCamera.Priority = unfocusPriority;

        detailsPanel.SetData(data);
        detailsPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        detection.OnIsFocusableChanged += OnIsFocusableChanged;
    }

    private void OnDisable()
    {
        detection.OnIsFocusableChanged -= OnIsFocusableChanged;
    }

    private void OnIsFocusableChanged(bool isFocusable)
    {
        canBeFocused = isFocusable;
        Debug.Log($"Focusable changed: {isFocusable}");
    }

    public void GetFocus()
    {
        if (focusCamera.Priority == focusPriority)
            return;

        focusCamera.Priority = focusPriority;
        detailsPanel.Show();
        Debug.Log($"{name} is getting your attention!!");
    }

    public void RemoveFocus()
    {
        if (focusCamera.Priority == unfocusPriority)
            return;

        focusCamera.Priority = unfocusPriority;
        detailsPanel.Hide();
        Debug.Log($"{name} is not interesting anymore :(");
    }
}
