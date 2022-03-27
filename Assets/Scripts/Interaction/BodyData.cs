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
    [Tooltip("The average radius of this body in **INSERT UNIT HERE**")]
    private float radius = 1.0f;

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
    [Tooltip("Generic description of this body, divided into paragraphs")]
    private string[] description;

    public string DisplayName => displayName;
    public float Radius => radius;
    public BodyMovementType MovementType => movementType;
    public OrbitData OrbitData => orbitData;
    public string[] Description => description;
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
}
