using TMPro;
using UnityEngine;

public class DetailsPanel : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField]
    private TMP_Text nameText;

    public void SetData(BodyData data)
    {
        nameText.text = data.DisplayName;
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
}
