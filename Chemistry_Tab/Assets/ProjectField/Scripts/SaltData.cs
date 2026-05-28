using UnityEngine;

[CreateAssetMenu(fileName = "SaltData", menuName = "SaltLab/SaltData")]
public class SaltData : ScriptableObject
{
    [Header("Identity")]
    public SaltType saltType;
    public string saltName;
    public string chemicalFormula;
    public Color saltColor = Color.white;

    [Header("Preliminary")]
    public string preliminaryObservation = "White crystalline solid";

    [Header("Watch Glass")]
    public Color saltParticleColor = Color.white;
    public float fillDuration = 1.2f;

    [Header("Flame Test")]
    public bool hasFlameColor = true;
    public Color flameColor = Color.yellow;
    public string flameObservation = "Yellow flame";
    public float flameDuration = 3f;

    [Header("Gas Test")]
    public bool producesGas = false;
    public GasType gasType = GasType.None;
    public Color gasColor = Color.clear;
    public string gasName = "";
    public string gasObservation = "No gas produced";
    public float gasDuration = 2.5f;

    [Header("Confirmatory Test")]
    public string confirmatoryReagent = "BaCl2 solution";
    public Color confirmatoryReactionColor = Color.white;
    public string confirmatoryObservation = "No precipitate";
    public bool formsPrecipitate = false;
    public float confirmatoryDuration = 2f;

    [Header("Inference")]
    public string cationInferred = "";
    public string anionInferred = "";
}