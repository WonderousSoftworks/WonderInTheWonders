using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] private float orbitalPeriod = 365.26f; // orbital period in earth days
    // see https://nssdc.gsfc.nasa.gov/planetary/factsheet/ for orbital periods used in the scene

    [SerializeField] private Vector3 axis = new Vector3(0, 1, 0);

    // this is a temp value to speed up the simulation, when a simulation speed controller
    // is implemented, this script should poll that instead
    [SerializeField] private float simulationSpeed = 30;

    [SerializeField] private SimulationSpeed simSpeedController;

    private float rotationRatio;

    // Start is called before the first frame update
    void Start()
    {
        // calculate this upfront so we don't have to do it every frame
        rotationRatio = (360 / orbitalPeriod);
    }

    // Update is called once per frame
    void Update()
    {
        if (simSpeedController)
        {
            // use the controller's speed instead
            float rotation = rotationRatio * simSpeedController.orbitSpeed * Time.deltaTime;
            transform.Rotate(axis.x * rotation, axis.y * rotation, axis.z * rotation);
            return;
        }

        float amount = rotationRatio * simulationSpeed * Time.deltaTime;
        transform.Rotate(axis.x * amount, axis.y * amount, axis.z * amount);
    }
}
