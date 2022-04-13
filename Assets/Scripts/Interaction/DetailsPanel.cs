using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class DetailsPanel : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private TMP_Text descriptionText;

    [SerializeField]
    private TMP_Text radius;

    [SerializeField]
    private TMP_Text distanceFSun;

    [SerializeField]
    private TMP_Text gravity;

    [SerializeField]
    private TMP_Text rotPeriod;

    public void SetData(BodyData data)
    {
        nameText.text = data.DisplayName;
        descriptionText.text = data.Description;
        radius.text = data.Radius.ToString();
        distanceFSun.text = data.DistanceFromSun.ToString();
        gravity.text = data.Gravity.ToString();
        rotPeriod.text = data.OrbitData.period.ToString();
        // set more stuff here
    }

    public void Show()
    {
        // TODO we could create some effects like sliding in from the right side or whatever
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();

        if (canvas == null)
        {
            Debug.LogError($"Oh no where did you throw away the canvas for {name} :'(");
            return;
        }

        RotationConstraint constraint = GetComponentInChildren<RotationConstraint>();

        if (constraint == null)
        {
            Debug.LogError($"Oh no where did you throw away the rotation constraint for {name} :'(");
            return;
        }

        if (constraint.sourceCount == 0)
        {
            Debug.LogError($"Oh no where did you throw away the sources of the rotation constraint for {name} :'(");
        }

        // Do not proceed if this script is run in the prefab where the game object is the root object
        if (transform.parent == null) return;

        if (canvas.worldCamera == null)
        {
            GameObject camObj = GameObject.Find("Main Camera");
            if (camObj != null) canvas.worldCamera = camObj.GetComponent<Camera>();
            if (canvas.worldCamera == null) Debug.LogError($"Failed to assign world camera to the canvas for {name}");
        }

        CinemachineVirtualCamera vCam = transform.parent.GetComponentInChildren<CinemachineVirtualCamera>();
        if (vCam.m_Follow == null || vCam.m_LookAt == null)
        {
            Transform target = transform.parent.GetChild(0);
            vCam.m_Follow = target;
            vCam.m_LookAt = target;
        }

        ConstraintSource source = constraint.sourceCount == 0 ? new ConstraintSource() : constraint.GetSource(0);
        source.weight = 1.0f;
        if (source.sourceTransform == null)
        {
            if (vCam != null)
            {
                source.sourceTransform = vCam.transform;
                constraint.SetSource(0, source);
            }
            else
            {
                Debug.LogError($"Oh no where is my virtual camera for {name} :'(");
            }
        }
    }
#endif
}
