using UnityEngine;
using SaltAnalysis.Core;

namespace SaltAnalysis.Data
{
    [CreateAssetMenu(fileName = "SessionManager", menuName = "SaltAnalysis/Session Manager")]
    public class SessionManager : ScriptableObject
    {
        public SaltType CurrentSalt { get; private set; } = SaltType.None;
        public int CurrentStep { get; private set; } = 0;
        public ExperimentType CurrentExperiment => (ExperimentType)CurrentStep;

        System.Action _onReset;

        void OnEnable()
        {
            CurrentSalt = SaltType.None;
            CurrentStep = 0;
            _onReset = null;
        }

        // ── Salt ─────────────────────────────────────────────────────────────

        public bool TrySetSalt(SaltType newSalt)
        {
            if (CurrentSalt == newSalt) return false;

            CurrentSalt = newSalt;
            CurrentStep = 0;
            _onReset?.Invoke();
            return true;
        }

        public void Clear()
        {
            CurrentSalt = SaltType.None;
            CurrentStep = 0;
            _onReset?.Invoke();
        }

        // ── Step ─────────────────────────────────────────────────────────────

        public bool IsStepAllowed(ExperimentType type)
        {
            return (int)type == CurrentStep;
        }

        public void IncrementStep()
        {
            CurrentStep++;
        }

        // ── Reset callback ───────────────────────────────────────────────────

        public void RegisterResetCallback(System.Action callback)
        {
            _onReset = callback;
        }
    }
}