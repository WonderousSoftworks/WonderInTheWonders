using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// component that makes the object follow another object
public class FollowObject : MonoBehaviour
{
    [SerializeField] private GameObject target;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position;
    }
}
