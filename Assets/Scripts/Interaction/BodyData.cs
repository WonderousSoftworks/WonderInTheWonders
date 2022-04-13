using UnityEngine;

[CreateAssetMenu(fileName = "Some Planet", menuName = "Data/Body Data", order = 0)]
public class BodyData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField]
    [Tooltip("The name of this body")]
    private string displayName;

    [SerializeField]
    [Min(0.0f)]
    [Tooltip("The average radius of this body in Kms")]
    private float radius = 6378.0f;

    [SerializeField]
    [Min(0.0f)]
    [Tooltip("The distance form the sun in Kmx10^6")]
    private float distanceFromSun = 149.6f;

    [SerializeField]
    [Min(0.0f)]
    [Tooltip("The gravity of this massive object in m/s^2")]
    private float gravity = 9.8f;

    [SerializeField]
    [Min(0.0f)]
    [Tooltip("The rotation in its own axix in hours")]
    private float rotationPeriod = 23.9f;

    [Header("Movement")]
    [SerializeField]
    [Tooltip("Type of movement")]
    private BodyMovementType movementType;

    [SerializeField]
    [Tooltip("The orbital period in earth days")]
    private OrbitData orbitData;

    [Header("Other")]
    [SerializeField]
    [TextArea]
    [Tooltip("Generic description of this body")]
    private string description;

    public string DisplayName => displayName;
    public float DistanceFromSun => distanceFromSun;
    public float Gravity => gravity;
    public float Radius => radius;
    public BodyMovementType MovementType => movementType;
    public OrbitData OrbitData => orbitData;
    public string Description => description;
}

public enum BodyMovementType
{
    None,
    Orbit,
}

[System.Serializable]
public struct OrbitData
{
    [Min(0.0f)]
    [Tooltip("How far away the orbit is from the center")]
    public float distance;

    [Min(0.0f)]
    [Tooltip("The orbital period in earth days")]
    public float period;

    [Min(0.0f)]
    [Tooltip("The orbital velocity in Km/s")]
    public float velocity;
}
