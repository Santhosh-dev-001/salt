using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Core;
using SaltAnalysis.Data;
using SaltAnalysis.Interface;
using SaltAnalysis.Interaction;

namespace SaltAnalysis.Zones
{
    public abstract class ExperimentZoneBase : MonoBehaviour, IExperimentZone
    {
        [Header("Zone Identity")]
        [SerializeField] string _zoneId;

        [Header("References")]
        [SerializeField] protected SaltRegistry _registry;
        [SerializeField] protected SessionManager _session;
        [SerializeField] protected DraggableRegistry _draggableRegistry;

        public string ZoneId => _zoneId;

        public abstract ExperimentType ExperimentType { get; }

        int _currentInteractionStep;
        ExperimentEffectData _effectData;
        bool _hasAnimation;

        // ── IExperimentZone ──────────────────────────────────────────────────

        public bool IsStepAllowed(SessionManager session)
        {
            return session.IsStepAllowed(ExperimentType);
        }

        public void Execute(SaltType salt)
        {
            SaltAnalysisData saltData = _registry.Get(salt);
            if (saltData == null)
            {
                Debug.LogWarning($"[{GetType().Name}] No data found for {salt}");
                return;
            }

            _effectData = saltData.GetEffectData(ExperimentType);
            if (_effectData == null || _effectData.steps == null || _effectData.steps.Count == 0)
            {
                Debug.LogWarning($"[{GetType().Name}] No steps defined for {salt}");
                return;
            }

            _currentInteractionStep = 0;
            SetupStep(_currentInteractionStep);
        }

        // ── Step management ──────────────────────────────────────────────────

        void SetupStep(int index)
        {
            if (index >= _effectData.steps.Count)
            {
                // all interaction steps done — move to next experiment
                _session.IncrementStep();
                return;
            }

            InteractionStepData step = _effectData.steps[index];

            // disable all draggables then enable only the one for this step
            _draggableRegistry.SetAllInteractable(false);
            _draggableRegistry.SetInteractable(step.draggableId, true);
        }

        public void OnStepDropped(string draggableId, string dropTargetId)
        {
            if (_effectData == null) return;
            if (_currentInteractionStep >= _effectData.steps.Count) return;

            InteractionStepData step = _effectData.steps[_currentInteractionStep];

            // validate draggable and drop target IDs
            if (step.draggableId != draggableId)
            {
                Debug.Log($"[{GetType().Name}] Wrong draggable. Expected {step.draggableId}");
                return;
            }

            if (step.dropTargetId != dropTargetId)
            {
                Debug.Log($"[{GetType().Name}] Wrong drop target. Expected {step.dropTargetId}");
                return;
            }

            // fire effects on the drop zone GO
            var effects = GetComponents<IDropEffect>();

            // check if animation is involved — it drives step completion
            _hasAnimation = step.playAnimation;

            if (_hasAnimation)
            {
                // register callback on OnDropAnimation
                var anim = GetComponent<OnDropAnimation>();
                if (anim != null)
                    anim.RegisterStepCompleteCallback(OnInteractionStepComplete);
            }

            foreach (var effect in effects)
                effect.Execute(step);

            // if no animation — complete step immediately after drop
            if (!_hasAnimation)
                OnInteractionStepComplete();
        }

        void OnInteractionStepComplete()
        {
            _currentInteractionStep++;
            SetupStep(_currentInteractionStep);
        }
    }
}