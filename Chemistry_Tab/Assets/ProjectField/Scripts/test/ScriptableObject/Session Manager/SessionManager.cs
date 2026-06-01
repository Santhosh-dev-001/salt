using UnityEngine;
using SaltAnalysis.Core;

namespace SaltAnalysis.Data
{
    [CreateAssetMenu(fileName = "SessionManager", menuName = "SaltAnalysis/Session Manager")]
    public class SessionManager : ScriptableObject
    {
        [Header("Runtime State — Read Only")]
        [SerializeField] SaltType _currentSalt;
        [SerializeField] int _currentStep;
        [SerializeField] ExperimentType _currentExperiment;

        public SaltType CurrentSalt => _currentSalt;
        public int CurrentStep => _currentStep;
        public ExperimentType CurrentExperiment => _currentExperiment;

        System.Action _onReset;

        void OnEnable()
        {
            _currentSalt = SaltType.None;
            _currentStep = 0;
            _currentExperiment = ExperimentType.PreliminaryExamination;
        }

        public bool TrySetSalt(SaltType newSalt)
        {
            if (_currentSalt == newSalt) return false;
            _currentSalt = newSalt;
            _currentStep = 0;
            _currentExperiment = (ExperimentType)_currentStep;
            _onReset?.Invoke();
            return true;
        }

        public bool IsStepAllowed(ExperimentType type)
        {
            return (int)type == _currentStep;
        }

        public void IncrementStep()
        {
            _currentStep++;
            _currentExperiment = (ExperimentType)_currentStep;
        }

        public void RegisterResetCallback(System.Action callback)
        {
            _onReset = callback;
        }

        public void Clear()
        {
            _currentSalt = SaltType.None;
            _currentStep = 0;
            _currentExperiment = ExperimentType.PreliminaryExamination;
            _onReset?.Invoke();
        }
    }
}