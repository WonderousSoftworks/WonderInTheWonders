using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystemMap : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject labelGroup;

    bool isActive = false;
    bool showLabels = true;

    // Start is called before the first frame update
    void Start()
    {
        map.SetActive(false);
    }

    public void ToggleActive()
    {
        isActive = !isActive;
        map.SetActive(isActive);
    }

    public void ToggleLabels()
    {
        showLabels = !showLabels;
        labelGroup.SetActive(showLabels);
    }
}
