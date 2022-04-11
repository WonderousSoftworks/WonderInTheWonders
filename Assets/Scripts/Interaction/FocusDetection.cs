using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class FocusDetection : MonoBehaviour
{
    [System.Serializable]
    public class OnIsFocusableChangedEvent : UnityEvent<bool> {}

    [SerializeField]
    private FocusSettings settings;

    [SerializeField]
    [Min(0.0f)]
    [Tooltip("Overrides the maximum distance at which this object can be focused on. Set to 0 to disable overriding.")]
    private float maxDistanceOverride;

    [SerializeField]
    private OnIsFocusableChangedEvent onIsFocusableChanged;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField]
    [Tooltip("Do NOT manually change this. Use the property IsFocusable instead.")]
#endif
    private bool isFocusable; // Just the underlying field for the property below

    /// <summary>
    ///   <para>Is this object currently focusable?</para>
    ///   <para>
    ///     Convenience property to automatically notify any subscribers of the change. Use this instead of the field
    ///     starting with a lower case.
    ///   </para>
    /// </summary>
    public bool IsFocusable
    {
        get => isFocusable;
        private set
        {
            if (isFocusable == value) return;
            isFocusable = value;
            onIsFocusableChanged?.Invoke(isFocusable);
        }
    }

    public event UnityAction<bool> OnIsFocusableChanged
    {
        add => onIsFocusableChanged.AddListener(value);
        remove => onIsFocusableChanged.RemoveListener(value);
    }

    private float MaxDistance => settings && maxDistanceOverride <= 0.0f ? settings.MaxDistance : maxDistanceOverride;

    // NOTE: This script has a script execution order of -150. You can check it in the project settings. This enables us
    // to check if this object is in the player's view before the input event to move the focus is processed. The input
    // processing has a script execution order of -100.
    // TODO: Use `OnBecameVisible` and `OnBecameInvisible` to further optimize?
    private void Update()
    {
        // The player camera at current frame
        CinemachineVirtualCamera playerCamera = PlayerFocusController.ShipCamera;

        // Not visible if the camera is not set or the camera is not currently being used
        if (playerCamera == null || !CinemachineCore.Instance.IsLive(playerCamera))
        {
            // Debug.Log($"{name} not focusable due to invalid camera");
            IsFocusable = false;
            return;
        }

        // The position of this object in the camera's local space
        // NOTE: This does not work properly if the camera has a world scale other than (1.0, 1.0, 1.0).
        Vector3 thisPositionWorld = transform.position;
        Vector3 thisPositionToCamera = playerCamera.transform.worldToLocalMatrix *
                                       new Vector4(thisPositionWorld.x, thisPositionWorld.y, thisPositionWorld.z, 1.0f);
        Debug.Log(thisPositionToCamera);

        // Properties of the camera
        LensSettings lensSettings = playerCamera.m_Lens;

        // Check if the object is in front of the camera and not behind the far clip plane
        if (thisPositionToCamera.z <= lensSettings.NearClipPlane ||
            thisPositionToCamera.z > Mathf.Min(MaxDistance, lensSettings.FarClipPlane))
        {
            // Debug.Log($"{name} not focusable due to being outside the clip planes");
            IsFocusable = false;
            return;
        }

        // The FOV angles in radians
        float verticalFovAngle = lensSettings.FieldOfView * Mathf.Deg2Rad / 2.0f; // Half the value, actually
        float horizontalFovAngle = verticalFovAngle * lensSettings.Aspect; // Half the value, actually

        // Is the object inside the horizontal FOV angle range?
        float tangentOfHorizontalAngle = thisPositionToCamera.x / thisPositionToCamera.z; // Can be negative
        if (Mathf.Abs(tangentOfHorizontalAngle) > Mathf.Tan(horizontalFovAngle))
        {
            // Debug.Log($"{name} not focusable due to being outside the horizontal FOV");
            IsFocusable = false;
            return;
        }

        // Is the object inside the vertical FOV angle range?
        float sineOfVerticalAngle = thisPositionToCamera.y / thisPositionToCamera.magnitude;
        if (Mathf.Abs(sineOfVerticalAngle) > Mathf.Sin(verticalFovAngle))
        {
            // Debug.Log($"{name} not focusable due to being outside the vertical FOV");
            IsFocusable = false;
            return;
        }

        // After all those checks, we can finally say that this object is indeed focusable
        IsFocusable = true;
    }
}
