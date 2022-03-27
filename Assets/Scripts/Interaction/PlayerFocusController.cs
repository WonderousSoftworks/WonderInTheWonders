using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction
{
    [DisallowMultipleComponent]
    public class PlayerFocusController : MonoBehaviour
    {
        [SerializeField]
        private GameObject overlayCamera;

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
    }
}
