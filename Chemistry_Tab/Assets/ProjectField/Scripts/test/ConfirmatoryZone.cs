using UnityEngine;
using SaltAnalysis.Core;
using SaltAnalysis.Data;
using SaltAnalysis.Interface;

namespace SaltAnalysis.Zones
{
    public class ConfirmatoryZone : MonoBehaviour, IExperimentZone
    {
        [SerializeField] SaltRegistry _registry;

        public ExperimentType ExperimentType => ExperimentType.ConfirmatoryTest;

        public void Execute(SaltType salt)
        {
            SaltAnalysisData data = _registry.Get(salt);
            if (data == null)
            {
                Debug.LogWarning($"[ConfirmatoryZone] No data found for {salt}");
                return;
            }

            ShowConfirmatoryResult(data);
        }

        void ShowConfirmatoryResult(SaltAnalysisData data)
        {
            // Drive your UI / animation here using:
            // data.confirmatoryReagent      — reagent added to confirm
            // data.precipitateColour        — colour of precipitate formed
            // data.confirmatoryObservation  — what was observed
            // data.confirmatoryInference    — final conclusion text

            Debug.Log($"[Confirmatory] {data.saltName} — Reagent: {data.confirmatoryReagent}, " +
                      $"Precipitate: {data.precipitateColour}, " +
                      $"Observation: {data.confirmatoryObservation}, " +
                      $"Inference: {data.confirmatoryInference}");
        }
    }
}
