using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Interface;
using SaltAnalysis.Data;

namespace SaltAnalysis.Interaction
{
    public class OnDropAnimation : MonoBehaviour, IDropEffect
    {
        [SerializeField] string[] _ids;
        [SerializeField] List<Animator> _animators;

        // zone registers itself to receive step complete callback
        System.Action _onStepComplete;

        public void RegisterStepCompleteCallback(System.Action callback)
        {
            _onStepComplete = callback;
        }

        public void Execute(InteractionStepData step)
        {
            if (!step.playAnimation) return;
            if (!IdMatches()) return;
            if (step.animatorController == null) return;
            if (string.IsNullOrEmpty(step.animationClip)) return;

            // find animator that matches the controller in step data
            Animator target = FindAnimator(step.animatorController);
            if (target == null)
            {
                Debug.LogWarning("[OnDropAnimation] No matching animator found.");
                _onStepComplete?.Invoke(); // no animator found — dont block step
                return;
            }

            target = step.animatorController;
            target.enabled = true;
            target.Play(step.animationClip);
        }

        // called by Animation Event on last frame of clip
        public void OnAnimationComplete()
        {
            // disable all animators that were playing
            foreach (var a in _animators)
                if (a != null && a.enabled)
                    a.enabled = false;

            _onStepComplete?.Invoke();
        }

        Animator FindAnimator(Animator controller)
        {
            foreach (var a in _animators)
                if (a != null && a.runtimeAnimatorController == controller)
                    return a;

            // if none matched — return first available
            foreach (var a in _animators)
                if (a != null) return a;

            return null;
        }

        bool IdMatches()
        {
            if (Draggable.CurrentlyDragged == null) return false;
            foreach (var id in _ids)
                if (id == Draggable.CurrentlyDragged.Id) return true;
            return false;
        }
    }
}