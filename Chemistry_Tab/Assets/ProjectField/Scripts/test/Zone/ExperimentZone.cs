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

        // ── Drop entry point ─────────────────────────────────────────────────

        public bool TryDrop(Draggable draggable)
        {
            Debug.Log("TryDrop Enter");

            if (!_session.IsStepAllowed(_experimentType))
            {
                Debug.Log("FAILED: Step not allowed");
                return false;
            }

            SaltData match = FindSaltData(draggable.saltType);

            if (match == null)
            {
                Debug.Log("FAILED: No SaltData");
                return false;
            }

            Debug.Log("CurrentInteractionStep = " +
                      _session.CurrentStep);

            InteractionStep step =
                _currentSaltData.GetStep(_session.CurrentStep);

            if (step == null)
            {
                Debug.Log("FAILED: Step is NULL");
                return false;
            }

            Debug.Log("Interaction Type = " + step.interactionType);

            FireEffects(step);

            return true;

        }

        // ── Step complete ─────────────────────────────────────────────────────

        public void OnStepComplete()
        {
            
            _session.CurrentStep++;
            if (_session.CurrentStep >= _currentSaltData.StepCount)
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
            Debug.Log("SetupStep" + _session.CurrentStep);
            if (_currentSaltData == null) return;

            InteractionStep step = _currentSaltData.GetStep(_session.CurrentStep);
            if (step == null) return;

            if (step.interactionType == InteractionType.Drag)
            {
                _clickableRegistry?.SetAllInteractable(false);
                _draggableRegistry.SetupForStep(_session.CurrentStep);
            }
            else if (step.interactionType == InteractionType.Click)
            {
                _draggableRegistry.SetAllInteractable(false);
                _clickableRegistry?.SetupForStep(_session.CurrentStep);
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
                {
                  
                    OnStepComplete();
                }
                    
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
            InteractionStep step = match.GetStep(_session.CurrentStep);
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
                _session.CurrentStep = 0;
               // Debug.Log(_currentSaltData);
                SetupStep();
            }

            InteractionStep step = _currentSaltData.GetStep(_session.CurrentStep);
            if (step == null) return false;
           // Debug.Log(step);
           // Debug.Log(step.interactionType.ToString());
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
           // Debug.Log(effects.Length);
            int total = 0;
            foreach (var effect in effects)
            {
              //  Debug.Log("Effect" + effect.GetType().Name);
              //  Debug.Log("IsFlagged" + effect.IsFlagged(step));
                if (effect.IsFlagged(step)) total++;
            }
                

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