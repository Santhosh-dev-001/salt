using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class SaltLabManager : MonoBehaviour
{
    public static SaltLabManager Instance { get; private set; }

    [Header("Raycast")]
    public Camera labCamera;
    public LayerMask saltLayer;
    public LayerMask equipmentLayer;

    [Header("State")]
    public SaltType selectedSalt = SaltType.None;
    public TestType currentTest = TestType.None;
    public bool saltSelected = false;

    [Header("Salt Data")]
    public List<SaltData> allSaltData = new List<SaltData>();

    [Header("Watch Glass")]
    public WatchGlassFill watchGlass;

    [Header("UI")]
    public TMPro.TextMeshProUGUI observationText;
    public TMPro.TextMeshProUGUI saltNameText;
    public TMPro.TextMeshProUGUI testNameText;
    public TMPro.TextMeshProUGUI inferenceText;

    [Header("Events")]
    public SaltSelectedEvent OnSaltSelected;
    public TestCompletedEvent OnTestCompleted;
    public UnityEvent OnSelectionCleared;

    private Dictionary<SaltType, SaltData> _saltDict = new();
    private SaltBottle _selectedBottle;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        foreach (var d in allSaltData)
            if (d != null) _saltDict[d.saltType] = d;
    }

    private void Start()
    {
        if (labCamera == null) labCamera = Camera.main;
    }

    private void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            if (t.phase == TouchPhase.Began)
                ProcessRay(labCamera.ScreenPointToRay(t.position));
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            ProcessRay(labCamera.ScreenPointToRay(Input.mousePosition));
#endif
    }

    private void ProcessRay(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit saltHit, Mathf.Infinity, saltLayer))
        {
            var target = saltHit.collider.GetComponent<ISaltInteractable>();
            if (target != null && target.GetState() == AnimationState.Idle)
            {
                HandleSaltSelected(target);
                target.OnTouch();
                return;
            }
        }

        if (Physics.Raycast(ray, out RaycastHit eqHit, Mathf.Infinity, equipmentLayer))
        {
            var eq = eqHit.collider.GetComponent<ILabEquipment>();
            if (eq != null)
            {
                if (!saltSelected)
                {
                    ShowObservation("Select a salt first!");
                    return;
                }
                eq.OnInteract(selectedSalt, GetCurrentSaltData());
            }
        }
    }

    private void HandleSaltSelected(ISaltInteractable target)
    {
        _selectedBottle?.Deselect();
        selectedSalt = target.GetSaltType();
        saltSelected = true;
        currentTest = TestType.PreliminaryExamination;
        _selectedBottle = target as SaltBottle;

        SaltData data = GetCurrentSaltData();

        if (saltNameText != null)
            saltNameText.text = $"{data?.saltName}  ({data?.chemicalFormula})";
        if (testNameText != null)
            testNameText.text = "Preliminary Examination";

        ShowObservation(data?.preliminaryObservation ?? "");
        ShowInference("");

        watchGlass?.FillWithSalt(data);
        OnSaltSelected?.Invoke(selectedSalt, data);
    }

    public SaltData GetCurrentSaltData()
    {
        _saltDict.TryGetValue(selectedSalt, out SaltData d);
        return d;
    }

    public void ShowObservation(string text)
    {
        if (observationText != null) observationText.text = text;
    }

    public void ShowInference(string text)
    {
        if (inferenceText != null) inferenceText.text = text;
    }

    public void SetTestName(string name)
    {
        if (testNameText != null) testNameText.text = name;
    }

    public void ReportTestComplete(TestType test, string observation, string inference = "")
    {
        currentTest = test;
        ShowObservation(observation);
        ShowInference(inference);
        OnTestCompleted?.Invoke(test, selectedSalt, observation);
    }

    public void ClearSelection()
    {
        selectedSalt = SaltType.None;
        saltSelected = false;
        _selectedBottle?.Deselect();
        _selectedBottle = null;
        watchGlass?.ClearGlass();
        OnSelectionCleared?.Invoke();
    }
}