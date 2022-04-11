using UnityEngine;

[CreateAssetMenu(fileName = "FocusSettings", menuName = "Config/Focus", order = 1)]
public class FocusSettings : ScriptableObject
{
    [SerializeField]
    [Min(0.0f)]
    [Tooltip("The maximum distance a body can be focused on. This can be overridden by individual focusable objects. " +
             "See field `maxDistanceOverride` in `FocusDetection` behaviour.")]
    private float maxDistance = 10_000;

    public float MaxDistance => maxDistance;
}
