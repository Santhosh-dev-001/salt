using UnityEngine;
using SaltAnalysis.Core;
using SaltAnalysis.Data;
using SaltAnalysis.Interface;

namespace SaltAnalysis.Zones
{
    public class FlameTestZone : MonoBehaviour, IExperimentZone
    {
        [SerializeField] SaltRegistry _registry;

        public ExperimentType ExperimentType => ExperimentType.FlameTest;

        public void Execute(SaltType salt)
        {
            SaltAnalysisData data = _registry.Get(salt);
            if (data == null)
            {
                Debug.LogWarning($"[FlameTestZone] No data found for {salt}");
                return;
            }

            ShowFlameResult(data);
        }

        void ShowFlameResult(SaltAnalysisData data)
        {
            if (data.showsFlameColour)
            {
                // Drive your flame particle system / material tint here using:
                // data.flameColour      — Unity Color, apply to particle system or material
                // data.flameColourName  — human-readable result for UI card

                Debug.Log($"[FlameTest] {data.saltName} — Flame colour: {data.flameColourName}");
            }
            else
            {
                // No characteristic colour — show default / no-colour response
                Debug.Log($"[FlameTest] {data.saltName} — No characteristic flame colour");
            }
        }
    }
}
