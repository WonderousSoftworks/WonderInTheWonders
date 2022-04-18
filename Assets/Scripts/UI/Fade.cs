using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Fade : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField]
    [Min(0.01f)]
    private float speed = 1.0f;

    [SerializeField]
    private bool shouldShow;

    private float TargetAlpha => shouldShow ? 1.0f : 0.0f;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGroup.alpha = TargetAlpha;
    }

    private void OnDisable()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        canvasGroup.alpha = TargetAlpha;
    }

    public void SetVisibility(bool show)
    {
        if (show == shouldShow) return;

        shouldShow = show;

        if (currentCoroutine != null) return;

        currentCoroutine = StartCoroutine(AlphaCoroutine());
    }

    private IEnumerator AlphaCoroutine()
    {
        canvasGroup.alpha += (shouldShow ? speed : -speed) * Time.deltaTime;

        while (canvasGroup.alpha > 0.0f && canvasGroup.alpha < 1.0f)
        {
            yield return null;
            canvasGroup.alpha += (shouldShow ? speed : -speed) * Time.deltaTime;
        }

        canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha);
        currentCoroutine = null;
    }
}
