using Cinemachine;
using UnityEngine;

namespace Interaction
{
    [DisallowMultipleComponent]
    public class Focusable : MonoBehaviour
    {
        [Header("Zoom-in focus control")]
        [SerializeField]
        private CinemachineVirtualCamera focusCamera;
        [SerializeField]
        private int focusPriority = 20;
        [SerializeField]
        private int unfocusPriority = 0;

        public CinemachineVirtualCamera FocusCamera => focusCamera;

        public void GetFocus()
        {
            if (focusCamera.Priority == focusPriority)
                return;

            focusCamera.Priority = focusPriority;
            Debug.Log($"{name} is getting your attention!!");
        }

        public void RemoveFocus()
        {
            if (focusCamera.Priority == unfocusPriority)
                return;

            focusCamera.Priority = unfocusPriority;
            Debug.Log($"{name} is not interesting anymore :(");
        }
    }
}
