using UnityEngine;
using UnityEngine.Events;

public class PressTabMessageController : MonoBehaviour
{
    [System.Serializable]
    public class OnShouldShowChangedEvent : UnityEvent<bool> {}

    [SerializeField]
    private Camera raycastCamera;

    [SerializeField]
    [Min(1.0f)]
    private float maxDistance = 10_000.0f;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private OnShouldShowChangedEvent onShouldShowChanged;

    private bool ShouldShowMessage
    {
        get => shouldShowMessage;
        set
        {
            if (shouldShowMessage == value) return;
            shouldShowMessage = value;
            onShouldShowChanged?.Invoke(value);
        }
    }
    private bool shouldShowMessage;

    private bool doNotShow;

    private void FixedUpdate()
    {
        if (doNotShow) return;

        Ray ray = raycastCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1.0f));
        bool isHit = Physics.Raycast(ray, maxDistance, layerMask);
        ShouldShowMessage = isHit;
    }

    public void OnFocusStateChanged(PlayerFocusController.FocusStateInfo focusState)
    {
        doNotShow = focusState.current != null;
        ShouldShowMessage = !doNotShow;
    }
}
