using System.Collections;
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

    [SerializeField]
    private FocusTrigger focusTrigger;

    [Header("Debug")]
    [SerializeField]
    [Tooltip("Do NOT change this manually. Use `Focused` property instead.")]
    private Focusable focused;

    [SerializeField]
    [Tooltip("Do NOT change this manually in the inspector.")]
    private List<Focusable> potentialFocusList = new List<Focusable>();

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

    // NOTE: This cannot be initialized right away because it requires a reference to the transform of `focusTrigger`
    private FocusableComparer comparer;

    private void Awake()
    {
        // A hack to get the reference to the camera for the ship from anywhere
        ShipCamera = shipCamera;

        // Able to initialize here because we assign the reference in the inspector and the value is serialized
        comparer = new FocusableComparer(focusTrigger.transform);
    }

    private void Start()
    {
        overlayCamera.SetActive(false);
    }

    #region Methods / Player Input Handlers

    /// <summary>
    ///   <para>Player input handler for moving focus to the next item in the list</para>
    /// </summary>
    /// <param name="context">input action context</param>
    public void OnMoveFocus(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        MoveFocus(context.ReadValue<float>() > 0 ? MoveFocusDirection.Next : MoveFocusDirection.Previous);
    }

    /// <summary>
    ///   <para>Player input handler for clearing focus</para>
    /// </summary>
    /// <param name="context">input action context</param>
    public void OnClearFocus(InputAction.CallbackContext context)
    {
        if (!context.performed || Focused == null) return;

        ClearFocus();
    }

    #endregion // Methods / Player Input Handlers

    #region Methods / Focus Controls

    private enum MoveFocusDirection
    {
        Next,
        Previous,
    }

    private void MoveFocus(MoveFocusDirection direction)
    {
        // Don't focus on anything (or even clear the focus if already focused on something) if the list is empty
        if (potentialFocusList.Count == 0)
        {
            ClearFocus();
            return;
        }

        SortList();

        // The number of elements in the list that are actually focusable
        // We try to get the index of the first one that is not focusable, and if not found, we use the actual count.
        int focusableCount = potentialFocusList.FindIndex(f => !f.IsFocusable);
        if (focusableCount < 0) focusableCount = potentialFocusList.Count;

        // Nothing is focusable, so clear the focus
        if (focusableCount == 0)
        {
            ClearFocus();
            return;
        }

        int idx = Focused ? potentialFocusList.IndexOf(Focused) : -1; // It's ok to pass in null, but no need to
        int newIdx;

        if (idx < 0 || idx >= focusableCount)
        {
            // `idx` is -1 when
            // 1. nothing was focused on previously or
            // 2. the previous one is not in the list anymore.
            // `idx` is greater than or equal to `focusableCount` if the currently focused one somehow changed.
            // But it doesn't matter. We just have to return the first or the last one in the list.
            newIdx = direction == MoveFocusDirection.Next ? 0 : focusableCount - 1;
        }
        else
        {
            // Move on to the next/previous one in the list
            int change = direction == MoveFocusDirection.Next ? 1 : -1;
            newIdx = idx + change;
        }

        // If the new index is out of range, we've reached an end of the list. To indicate that, get out of the focus
        // state. We can change this later if wanted, but in that case, we need to check for the case where the new
        // index is identical to the previous one.
        if (newIdx < 0 || newIdx >= focusableCount)
        {
            ClearFocus();
            return;
        }

        // Finally assign what to focus on
        Focused = potentialFocusList[newIdx];
    }

    private void ClearFocus()
    {
        Focused = null;
    }

    private void SortList()
    {
        potentialFocusList.Sort(comparer);
    }

    #endregion // Methods / Focus Controls

    #region Methods / Focus Trigger Callbacks

    public void OnFocusableEnter(Focusable entered)
    {
#if UNITY_EDITOR
        if (potentialFocusList.Contains(entered)) return;
#endif
        potentialFocusList.Add(entered);
    }

    public void OnFocusableExit(Focusable exited)
    {
        potentialFocusList.Remove(exited);
    }

    #endregion // Methods / Focus Trigger Callbacks

    /// <summary>
    ///   <para>Custom comparer for `Focusable` objects, used to sort the focusable list</para>
    /// </summary>
    private readonly struct FocusableComparer : IComparer<Focusable>
    {
        private readonly Transform from;

        public FocusableComparer(Transform fromPosition)
        {
            from = fromPosition;
        }

        public int Compare(Focusable x, Focusable y)
        {
            // `null` comes last
            if (x == null && y == null) return 0; // `null` and `null` are equal
            if (x == null) return 1; // x is "greater"
            if (y == null) return -1; // x is "less"

            // Focusable ones should come first
            if (!x.IsFocusable && !y.IsFocusable) return 0; // non-focusable ones are equal
            if (!x.IsFocusable) return 1; // x is "greater"
            if (!y.IsFocusable) return -1; // x is "less"

            Vector3 fromPosition = from.position;
            float d1 = (x.transform.position - fromPosition).magnitude;
            float d2 = (y.transform.position - fromPosition).magnitude;
            // Debug.Log($"Compare {d1} and {d2}: {Comparer.Default.Compare(d1, d2)}");
            return Comparer.Default.Compare(d1, d2); // the one with smaller distance is "less"
        }
    }
}
