using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Core;
using SaltAnalysis.Interface;
using SaltAnalysis.Data;

namespace SaltAnalysis.Interaction
{
    public class OnDropAnimation : MonoBehaviour, IDropEffect
    {
        [System.Serializable]
        public class SaltEntry
        {
            public SaltType saltType;
            public string stringId;
            public Animator animator;
        }

        [SerializeField] List<SaltEntry> _entries = new();

        Dictionary<(SaltType, string), Animator> _map;

        System.Action _onComplete;

        void Awake()
        {
            _map = new();
            foreach (var e in _entries)
                if (e.animator != null)
                    _map[(e.saltType, e.stringId)] = e.animator;
        }

        public bool IsFlagged(InteractionStep step)
        {
            if (!step.stepEffects.HasFlag(StepEffects.Animation)) return false;
            if (Draggable.CurrentlyDragged == null) return false;
            return _map.ContainsKey((Draggable.CurrentlyDragged.saltType, Draggable.CurrentlyDragged.stringId));
        }

        public void Execute(InteractionStep step, System.Action onComplete)
        {
            if (!IsFlagged(step)) return;
            if (string.IsNullOrEmpty(step.animationClip)) { onComplete?.Invoke(); return; }

            var key = (Draggable.CurrentlyDragged.saltType, Draggable.CurrentlyDragged.stringId);
            if (!_map.TryGetValue(key, out Animator animator)) { onComplete?.Invoke(); return; }

            _onComplete = onComplete;

            animator.enabled = true;
            animator.Play(step.animationClip);
        }

        // called by Animation Event on last frame of clip
        public void OnAnimationComplete()
        {
            foreach (var e in _entries)
                if (e.animator != null && e.animator.enabled)
                    e.animator.enabled = false;

            _onComplete?.Invoke();
            _onComplete = null;
        }
    }
}