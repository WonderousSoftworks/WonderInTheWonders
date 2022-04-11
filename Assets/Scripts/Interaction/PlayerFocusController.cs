using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerFocusController : MonoBehaviour
{
    // A hack to get the reference to the camera for the ship from anywhere
    public static CinemachineVirtualCamera ShipCamera { get; private set; }

    [Header("Cameras")]
    [SerializeField]
    private GameObject overlayCamera;

    [SerializeField]
    private CinemachineVirtualCamera shipCamera;

    [Header("Settings")]
    [SerializeField]
    private FocusSettings focusSettings;

    [Header("Debug")]
    [SerializeField]
    private List<Focusable> potentialFocusList;

    // TODO implement proper selection for what to focus on
    public Focusable tmpFocus;

    /// <summary>
    ///   The currently focused object; null of nothing is focused
    /// </summary>
    public Focusable Focused
    {
        get => focused;
        private set
        {
            if (focused == value) return;

            overlayCamera.SetActive(value != null);
            if (focused != null) focused.RemoveFocus();
            if (value != null) value.GetFocus();

            focused = value;
        }
    }
    private Focusable focused;

    private void Start()
    {
        // A hack to get the reference to the camera for the ship from anywhere
        ShipCamera = shipCamera;

        overlayCamera.SetActive(false);
    }

    public void OnMoveFocus(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (context.ReadValue<float>() > 0)
        {
            Focused = tmpFocus;
        }
    }

    public void OnClearFocus(InputAction.CallbackContext context)
    {
        if (!context.performed || Focused == null) return;

        Focused = null;
    }

    private void MoveFocus() {}
}
