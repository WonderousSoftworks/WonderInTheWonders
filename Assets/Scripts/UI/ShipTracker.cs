using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTracker : MonoBehaviour
{
    public Transform ship;

    // just getting a ratio from world space to map space using the farthest planet in the scene
    [SerializeField] private float neptuneDistanceWorld;
    [SerializeField] private float neptuneDistanceMap;

    private float ratio;
    private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        ratio = neptuneDistanceMap / neptuneDistanceWorld;
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rt.anchoredPosition = new Vector3(ship.position.z * ratio, ship.position.x * ratio, 0);
    }
}
