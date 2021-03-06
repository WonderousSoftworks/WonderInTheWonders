using UnityEngine;

public class FocusOverlay : MonoBehaviour
{
    [SerializeField]
    private FocusNextIndicator nextIndicator;

    [SerializeField]
    private FocusNextIndicator previousIndicator;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnFocusStateChanged(PlayerFocusController.FocusStateInfo focusState)
    {
        gameObject.SetActive(focusState.current != null);
        nextIndicator.ShowState(focusState.next);
        previousIndicator.ShowState(focusState.previous);
    }
}
