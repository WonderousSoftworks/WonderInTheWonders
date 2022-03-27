﻿using Cinemachine;
using UnityEngine;

[DisallowMultipleComponent]
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

    public CinemachineVirtualCamera FocusCamera => focusCamera;

    private void Start()
    {
        focusCamera.Priority = unfocusPriority;
        detailsPanel.SetData(data);
    }

    public void GetFocus()
    {
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
}
