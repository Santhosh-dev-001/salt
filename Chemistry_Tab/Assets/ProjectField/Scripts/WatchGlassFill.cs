using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class WatchGlassFill : MonoBehaviour
{
    [Header("Salt Pile")]
    public GameObject saltPileObject;
    public Renderer saltPileRenderer;
    public Transform saltPileTransform;

    [Header("Particles")]
    public ParticleSystem pourParticles;

    [Header("Events")]
    public UnityEvent OnFillStarted;
    public UnityEvent OnFillFinished;

    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private Vector3 _originalScale;

    private void Awake()
    {
        if (saltPileTransform != null)
            _originalScale = saltPileTransform.localScale;
        saltPileObject?.SetActive(false);
    }

    public void FillWithSalt(SaltData data)
    {
        if (data == null) return;
        StopAllCoroutines();
        StartCoroutine(FillRoutine(data));
    }

    public void ClearGlass()
    {
        StopAllCoroutines();
        saltPileObject?.SetActive(false);
    }

    private IEnumerator FillRoutine(SaltData data)
    {
        OnFillStarted?.Invoke();
        saltPileObject?.SetActive(true);

        if (saltPileRenderer != null)
            saltPileRenderer.material.SetColor(BaseColor, data.saltParticleColor);

        pourParticles?.Play();

        if (saltPileTransform != null)
        {
            saltPileTransform.localScale = Vector3.zero;
            float t = 0f;
            while (t < data.fillDuration)
            {
                float e = Mathf.SmoothStep(0f, 1f, t / data.fillDuration);
                saltPileTransform.localScale = Vector3.LerpUnclamped(
                    Vector3.zero, _originalScale, e);
                t += Time.deltaTime;
                yield return null;
            }
            saltPileTransform.localScale = _originalScale;
        }
        else
        {
            yield return new WaitForSeconds(data.fillDuration);
        }

        pourParticles?.Stop();
        SaltLabManager.Instance?.ShowObservation(data.preliminaryObservation);
        OnFillFinished?.Invoke();
    }
}