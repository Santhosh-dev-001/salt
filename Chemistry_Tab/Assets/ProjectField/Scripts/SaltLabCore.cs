using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.Events;

public enum SaltType
{
    None,
    CopperSulphate,
    SodiumChloride,
    AmmoniumChloride,
    CalciumCarbonate,
    PotassiumNitrate
}

public enum TestType
{
    None,
    PreliminaryExamination,
    FlameTest,
    GasTest,
    ConfirmatoryTest
}

public enum AnimationState { Idle, Playing, Finished }

public enum GasType
{
    None,
    CarbonDioxide,
    AmmoniaGas,
    SulphurDioxide,
    NitrogenDioxide,
    HydrogenGas
}

public interface ISaltInteractable
{
    void OnTouch();
    AnimationState GetState();
    SaltType GetSaltType();
}

public interface ILabEquipment
{
    void OnInteract(SaltType salt, SaltData data);
    TestType GetTestType();
}

[System.Serializable] public class SaltSelectedEvent : UnityEvent<SaltType, SaltData> { }
[System.Serializable] public class TestCompletedEvent : UnityEvent<TestType, SaltType, string> { }