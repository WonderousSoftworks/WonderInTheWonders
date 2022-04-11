using UnityEngine;

[CreateAssetMenu(fileName = "FocusSettings", menuName = "Config/Focus", order = 0)]
public class FocusSettings : ScriptableObject
{
    [SerializeField]
    [Min(0.0f)]
    [Tooltip("The maximum distance a body can be focused on")]
    private float maxDistance = 10_000;

    public float MaxDistance => maxDistance;
}
