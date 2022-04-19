using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationSpeed : MonoBehaviour
{
    public int factor = 1;

    private int increaseFactor = 5;

    // roughly angular rotation where 1 second = 1 earth day
    // (360 / 365.26 d) * 360
    private const float orbitSpeedConstant = (360 / 365.26f);

    public float orbitSpeed = orbitSpeedConstant;

    private Text text;

    // functions for the UI
    public void Increase()
    {
        factor += increaseFactor;
        orbitSpeed = orbitSpeedConstant;
        text = GetComponent<Text>();

        RecalculateSpeed();
        UpdateText();
    }

    public void Decrease()
    {
        factor -= increaseFactor;
        if (factor <= 0)
        {
            factor = 0;
        }

        RecalculateSpeed();
        UpdateText();
    }

    private void RecalculateSpeed()
    {
        orbitSpeed = orbitSpeedConstant * factor;
    }

    private void UpdateText()
    {
        text.text = "Speed: " + factor + "x";
    }
}
