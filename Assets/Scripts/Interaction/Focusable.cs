using Cinemachine;
using Editor.Utils;
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

    public bool IsFocusable => detection.IsFocusable;

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
        Debug.Log($"Focusable changed for {name}: {isFocusable}");
    }

    public void GetFocus()
    {
        if (!detection.IsFocusable)
            Debug.LogWarning($"`GetFocus` called for {name}, but it is currently not focusable.\n" +
                             "Nothing is going to break, but something strange happened here.");

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

#if UNITY_EDITOR
    private void OnValidate()
    {
        FocusDetection det = GetComponent<FocusDetection>();
        if (det == null)
            Debug.LogError($"`Focusable` for {name} requires a `FocusDetection` component on the same game object");

        if (focusCamera == null)
        {
            focusCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            if (focusCamera == null) Debug.LogError($"`Focusable.focusCamera` is not assigned for {name}");
        }

        if (unfocusPriority >= focusPriority)
        {
            Debug.LogError("`Focusable.unfocusPriority` must be less than `Focusable.focusPriority`, but " +
                           $"{unfocusPriority} is greater than or equal to {focusPriority}");
        }

        if (data == null)
        {
            data = AssetHelper.GetAssetWithName<BodyData>(name, true);
            if (data != null)
                Debug.LogWarning($"`BodyData` for {name} automatically assigned to {AssetHelper.GetAssetPath(data)}");
            else
                Debug.LogError($"`Focusable.data` is not assigned for {name}");
        }

        if (detailsPanel == null)
        {
            detailsPanel = GetComponentInChildren<DetailsPanel>();
            if (detailsPanel == null) Debug.LogError($"`Focusable.detailsPanel` is not assigned for {name}");
        }

        SphereCollider col = GetComponentInChildren<SphereCollider>();
        if (col == null)
            Debug.LogError(
                $"`Focusable` for {name} requires a sphere collider with tag \"Focusable\" on a child object");
    }
#endif
}
