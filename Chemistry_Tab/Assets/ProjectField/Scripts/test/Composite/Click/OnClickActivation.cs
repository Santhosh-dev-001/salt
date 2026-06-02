using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Core;
using SaltAnalysis.Interface;
using SaltAnalysis.Data;

namespace SaltAnalysis.Interaction
{
    public class OnClickActivation : MonoBehaviour, IClickEffect
    {
        [System.Serializable]
        public class SaltEntry
        {
            public SaltType         saltType;
            public string           stringId;
            public List<GameObject> targets = new();
        }

        [SerializeField] List<SaltEntry> _entries = new();

        Dictionary<(SaltType, string), List<GameObject>> _map;

        void Awake()
        {
            _map = new();
            foreach (var e in _entries)
                _map[(e.saltType, e.stringId)] = e.targets;
        }

        public bool IsFlagged(InteractionStep step)
        {
            if (!step.stepEffects.HasFlag(StepEffects.Activation)) return false;
            if (Draggable.CurrentlyDragged == null) return false;
            return _map.ContainsKey((Draggable.CurrentlyDragged.saltType, Draggable.CurrentlyDragged.stringId));
        }

        public void Execute(InteractionStep step, System.Action onComplete)
        {
            if (!IsFlagged(step)) return;

            var key = (Draggable.CurrentlyDragged.saltType, Draggable.CurrentlyDragged.stringId);
            if (!_map.TryGetValue(key, out var targets)) return;

            foreach (var go in targets)
                if (go != null) go.SetActive(true);

            onComplete?.Invoke();
        }
    }
}
