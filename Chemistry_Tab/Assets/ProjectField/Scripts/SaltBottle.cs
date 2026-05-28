using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SaltBottle : MonoBehaviour, ISaltInteractable
{
    [Header("Identity")]
    public SaltType saltType;

    [Header("Animation")]
    public float riseHeight = 0.18f;
    public float riseDuration = 0.35f;
    public AnimationCurve riseCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Glow")]
    public Renderer bottleRenderer;
    public Color selectedEmission = new Color(0f, 1f, 1f, 1f);

    [Header("Events")]
    public UnityEvent OnBottleSelected;
    public UnityEvent OnBottleDeselected;

    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private AnimationState _state = AnimationState.Idle;
    private Vector3 _originalPos;

    private void Start() => _originalPos = transform.localPosition;

    public AnimationState GetState() => _state;
    public SaltType GetSaltType() => saltType;

    public void OnTouch()
    {
        if (_state != AnimationState.Idle) return;
        StartCoroutine(SelectRoutine());
        OnBottleSelected?.Invoke();
    }

    private IEnumerator SelectRoutine()
    {
        _state = AnimationState.Playing;
        yield return LerpLocal(_originalPos,
            _originalPos + Vector3.up * riseHeight, riseDuration);
        SetGlow(selectedEmission);
        _state = AnimationState.Finished;
        yield return new WaitForSeconds(0.2f);
        _state = AnimationState.Idle;
    }

    public void Deselect()
    {
        StopAllCoroutines();
        StartCoroutine(DeselectRoutine());
    }

    private IEnumerator DeselectRoutine()
    {
        yield return LerpLocal(transform.localPosition, _originalPos, riseDuration);
        SetGlow(Color.black);
        _state = AnimationState.Idle;
        OnBottleDeselected?.Invoke();
    }

    private IEnumerator LerpLocal(Vector3 from, Vector3 to, float dur)
    {
        float t = 0f;
        while (t < dur)
        {
            transform.localPosition = Vector3.LerpUnclamped(
                from, to, riseCurve.Evaluate(t / dur));
            t += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = to;
    }

    private void SetGlow(Color c)
    {
        if (bottleRenderer == null) return;
        bottleRenderer.material.EnableKeyword("_EMISSION");
        bottleRenderer.material.SetColor(EmissionColor, c);
    }
}