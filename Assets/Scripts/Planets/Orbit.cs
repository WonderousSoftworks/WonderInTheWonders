using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] private float orbitalPeriod = 365.26f; // orbital period in earth days
    // see https://nssdc.gsfc.nasa.gov/planetary/factsheet/ for orbital periods used in the scene

    [SerializeField] private Vector3 axis = new Vector3(0, 1, 0);

    private float earthYear = (360f / 365.26f) * 360;
    // this is a temp value to speed up the simulation, when a simulation speed controller
    // is implemented, this script should poll that instead
    private float simulationSpeed = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float amount = (earthYear / orbitalPeriod) * simulationSpeed * Time.deltaTime;
        transform.Rotate(axis.x * amount, axis.y * amount, axis.z * amount);
    }
}
