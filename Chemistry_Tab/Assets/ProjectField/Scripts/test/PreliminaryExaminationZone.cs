using UnityEngine;
using SaltAnalysis.Core;
using SaltAnalysis.Data;
using SaltAnalysis.Interface;

namespace SaltAnalysis.Zones
{
    public class PreliminaryExaminationZone : MonoBehaviour, IExperimentZone
    {
        [SerializeField] SaltRegistry _registry;

        public ExperimentType ExperimentType => ExperimentType.PreliminaryExamination;

        public void Execute(SaltType salt)
        {
            SaltAnalysisData data = _registry.Get(salt);
            if (data == null)
            {
                Debug.LogWarning($"[PreliminaryZone] No data found for {salt}");
                return;
            }

            ShowObservation(data);
        }

        void ShowObservation(SaltAnalysisData data)
        {
            // Drive your UI / animation here using:
            // data.physicalAppearance
            // data.colour
            // data.odour
            // data.isSoluble
            // data.solubilityNote

            Debug.Log($"[Preliminary] {data.saltName} — Appearance: {data.physicalAppearance}, " +
                      $"Colour: {data.colour}, Odour: {data.odour}, " +
                      $"Soluble: {data.isSoluble} ({data.solubilityNote})");
        }
    }
}
