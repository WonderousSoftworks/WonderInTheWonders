using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider))]
public class FocusTrigger : MonoBehaviour
{
    [System.Serializable]
    public class OnFocusableEnterEvent : UnityEvent<Focusable> {}

    [System.Serializable]
    public class OnFocusableExitEvent : UnityEvent<Focusable> {}

    [SerializeField]
    private OnFocusableEnterEvent onFocusableEnter;

    [SerializeField]
    private OnFocusableExitEvent onFocusableExit;

    public event UnityAction<Focusable> OnFocusableEnter
    {
        add => onFocusableEnter.AddListener(value);
        remove => onFocusableEnter.RemoveListener(value);
    }

    public event UnityAction<Focusable> OnFocusableExit
    {
        add => onFocusableExit.AddListener(value);
        remove => onFocusableExit.RemoveListener(value);
    }

    private BoxCollider trigger;

    private void Awake()
    {
        trigger = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        SetColliderSize(PlayerFocusController.ShipCamera.m_Lens);
    }

    // NOTE: The game object is assigned `FocusTrigger` layer, and that layer is set to only interact with `Focusable`
    // layer. Therefore, there is no need to check for the layer.
    private void OnTriggerEnter(Collider other)
    {
        // The object with the actual model and collider might not be the same object
        Focusable focusable = other.GetComponentInParent<Focusable>();

        if (focusable)
        {
            // Debug.Log($"Focusable from {focusable.name} entered trigger");
            onFocusableEnter?.Invoke(focusable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // The object with the actual model and collider might not be the same object
        Focusable focusable = other.GetComponentInParent<Focusable>();

        if (focusable)
        {
            // Debug.Log($"Focusable from {focusable.name} exited trigger");
            onFocusableExit?.Invoke(focusable);
        }
    }

    /// <summary>
    ///   <para>Adjust the trigger's size according to the new camera lens settings</para>
    ///   <para>
    ///     You must call this function after changing any properties for the lens settings of the virtual camera.
    ///   </para>
    /// </summary>
    /// <param name="lensSettings">new properties of the camera</param>
    public void SetColliderSize(LensSettings lensSettings)
    {
        float halfVerticalAngle = Mathf.Deg2Rad * lensSettings.FieldOfView / 2.0f;
        float halfHorizontalAngle = halfVerticalAngle * lensSettings.Aspect;

        float z = lensSettings.FarClipPlane;

        trigger.center = new Vector3(0.0f, 0.0f, z / 2.0f);
        trigger.size = new Vector3(
            2.0f * z * Mathf.Tan(halfHorizontalAngle),
            2.0f * z * Mathf.Tan(halfVerticalAngle),
            z);
    }
}
