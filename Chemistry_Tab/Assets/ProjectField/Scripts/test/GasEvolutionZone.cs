using UnityEngine;
using SaltAnalysis.Core;
using SaltAnalysis.Data;
using SaltAnalysis.Interface;

namespace SaltAnalysis.Zones
{
    public class GasEvolutionZone : MonoBehaviour, IExperimentZone
    {
        [SerializeField] SaltRegistry _registry;

        public ExperimentType ExperimentType => ExperimentType.GasEvolutionTest;

        public void Execute(SaltType salt)
        {
            SaltAnalysisData data = _registry.Get(salt);
            if (data == null)
            {
                Debug.LogWarning($"[GasEvolutionZone] No data found for {salt}");
                return;
            }

            ShowGasResult(data);
        }

        void ShowGasResult(SaltAnalysisData data)
        {
            // Drive your UI / animation here using:
            // data.reagentUsed    — what reagent was added
            // data.gasEvolved     — name of gas produced
            // data.gasColour      — colour of the gas
            // data.gasOdour       — smell of the gas
            // data.litmusEffect   — effect on litmus paper
            // data.gasInference   — conclusion text for result card

            Debug.Log($"[GasEvolution] {data.saltName} — Reagent: {data.reagentUsed}, " +
                      $"Gas: {data.gasEvolved}, Litmus: {data.litmusEffect}, " +
                      $"Inference: {data.gasInference}");
        }
    }
}
