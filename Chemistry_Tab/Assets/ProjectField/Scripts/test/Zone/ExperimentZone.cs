using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Core;
using SaltAnalysis.Data;
using SaltAnalysis.Interface;

namespace SaltAnalysis.Interaction
{
    public class ExperimentZone : MonoBehaviour
    {
        [Header("Identity")]
        [SerializeField] ExperimentType _experimentType;

        [Header("Salt Data")]
        [SerializeField] List<SaltData> _saltDataList = new();

        [Header("References")]
        [SerializeField] SessionManager _session;
        [SerializeField] DraggableRegistry _draggableRegistry;
        [SerializeField] ClickableRegistry _clickableRegistry;

        public ExperimentType ExperimentType => _experimentType;

        // ── Runtime ──────────────────────────────────────────────────────────

        SaltData _currentSaltData;
        int _currentStepIndex;

        // ── Drop entry point ─────────────────────────────────────────────────

        public bool TryDrop(Draggable draggable)
        {
            if (!_session.IsStepAllowed(_experimentType))
            {
                Debug.Log($"[ExperimentZone] {_experimentType} not allowed yet.");
                return false;
            }

            SaltData match = FindSaltData(draggable.saltType);
            if (match == null)
            {
                Debug.Log($"[ExperimentZone] No SaltData for {draggable.saltType}");
                return false;
            }

            if (_currentSaltData != match)
            {
                _currentSaltData = match;
                _currentStepIndex = 0;
                SetupStep();
            }

            InteractionStep step = _currentSaltData.GetStep(_currentStepIndex);
            if (step == null)
            {
                Debug.Log($"[ExperimentZone] No step at index {_currentStepIndex}");
                return false;
            }

            FireEffects(step);
            return true;
        }

        // ── Step complete ─────────────────────────────────────────────────────

        public void OnStepComplete()
        {
            _currentStepIndex++;

            if (_currentStepIndex >= _currentSaltData.StepCount)
            {
                Debug.Log($"[ExperimentZone] All steps complete for {_currentSaltData.saltType}");
                _draggableRegistry.SetAllInteractable(false);
                _session.IncrementStep();
                return;
            }

            SetupStep();
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        SaltData FindSaltData(SaltType saltType)
        {
            foreach (var sd in _saltDataList)
                if (sd != null && sd.saltType == saltType) return sd;
            return null;
        }

        void SetupStep()
        {
            if (_currentSaltData == null) return;

            InteractionStep step = _currentSaltData.GetStep(_currentStepIndex);
            if (step == null) return;

            if (step.interactionType == InteractionType.Drag)
            {
                _clickableRegistry?.SetAllInteractable(false);
                _draggableRegistry.SetupForStep(_currentStepIndex);
            }
            else if (step.interactionType == InteractionType.Click)
            {
                _draggableRegistry.SetAllInteractable(false);
                _clickableRegistry?.SetupForStep(_currentStepIndex);
            }
        }

        void FireEffects(InteractionStep step)
        {
            var effects = GetComponents<IDropEffect>();

            // count only effects that are flagged AND key matches
            int total = 0;
            foreach (var effect in effects)
                if (effect.IsFlagged(step)) total++;

            if (total == 0)
            {
                OnStepComplete();
                return;
            }

            int completed = 0;
            void OnEffectDone()
            {
                completed++;
                if (completed >= total)
                    OnStepComplete();
            }

            foreach (var effect in effects)
                effect.Execute(step, OnEffectDone);
        }

        // ── Click entry point ─────────────────────────────────────────────────


        public bool IsStepAllowed(Clickable clickable)
        {
            if (!_session.IsStepAllowed(_experimentType)) return false;
            SaltData match = FindSaltData(clickable.saltType);
            if (match == null) return false;
            InteractionStep step = match.GetStep(_currentStepIndex);
            if (step == null) return false;
            return step.interactionType == InteractionType.Click;
        }

        public bool TryClick(Clickable clickable)
        {
            if (!_session.IsStepAllowed(_experimentType)) return false;

            SaltData match = FindSaltData(clickable.saltType);
            if (match == null) return false;

            if (_currentSaltData != match)
            {
                _currentSaltData = match;
                _currentStepIndex = 0;
                SetupStep();
            }

            InteractionStep step = _currentSaltData.GetStep(_currentStepIndex);
            if (step == null) return false;

            if (step.interactionType != InteractionType.Click)
            {
                Debug.Log("[ExperimentZone] Current step is not a click step.");
                return false;
            }

            FireClickEffects(step);
            return true;
        }

        void FireClickEffects(InteractionStep step)
        {
            var effects = GetComponents<SaltAnalysis.Interface.IClickEffect>();

            int total = 0;
            foreach (var effect in effects)
                if (effect.IsFlagged(step)) total++;

            if (total == 0) { OnStepComplete(); return; }

            int completed = 0;
            void OnEffectDone()
            {
                completed++;
                if (completed >= total) OnStepComplete();
            }

            foreach (var effect in effects)
                effect.Execute(step, OnEffectDone);
        }
    }
}

// ── Click entry point — called by ClickInputController ───────────────────────────
// Partial extension — add this method to ExperimentZone class