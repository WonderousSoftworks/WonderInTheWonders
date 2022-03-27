using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction
{
    [DisallowMultipleComponent]
    public class PlayerFocusController : MonoBehaviour
    {
        public Focusable tmpFocus;

        public void OnMoveFocus(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            if (context.ReadValue<float>() > 0)
                tmpFocus.GetFocus();
            else
                tmpFocus.RemoveFocus();
        }
    }
}
