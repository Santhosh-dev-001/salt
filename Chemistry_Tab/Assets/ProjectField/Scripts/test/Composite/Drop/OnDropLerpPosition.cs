using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Core;
using SaltAnalysis.Interface;
using SaltAnalysis.Data;

namespace SaltAnalysis.Interaction
{
    public class OnDropLerpPosition : MonoBehaviour, IDropEffect
    {
        [System.Serializable]
        public class SaltEntry
        {
            public SaltType saltType;
            public string stringId;
            public List<Transform> targets = new();
        }

        [SerializeField] List<SaltEntry> _entries = new();

        Dictionary<(SaltType, string), List<Transform>> _map;

        void Awake()
        {
            _map = new();
            foreach (var e in _entries)
                _map[(e.saltType, e.stringId)] = e.targets;
        }

        public bool IsFlagged(InteractionStep step)
        {
            if (!step.stepEffects.HasFlag(StepEffects.LerpPosition)) return false;
            if (Draggable.CurrentlyDragged == null) return false;
            return _map.ContainsKey((Draggable.CurrentlyDragged.saltType, Draggable.CurrentlyDragged.stringId));
        }

        public void Execute(InteractionStep step, System.Action onComplete)
        {
            if (!IsFlagged(step)) return;

            var key = (Draggable.CurrentlyDragged.saltType, Draggable.CurrentlyDragged.stringId);
            if (!_map.TryGetValue(key, out var targets)) return;

            StartCoroutine(LerpAllPositions(targets, step, onComplete));
        }

        IEnumerator LerpAllPositions(List<Transform> targets, InteractionStep step, System.Action onComplete)
        {
            float elapsed = 0f;
            Vector3[] startPositions = new Vector3[targets.Count];

            for (int i = 0; i < targets.Count; i++)
                if (targets[i] != null)
                    startPositions[i] = step.useLocalSpace
                        ? targets[i].localPosition
                        : targets[i].position;

            while (elapsed < step.lerpPositionDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / step.lerpPositionDuration;
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i] == null) continue;
                    if (step.useLocalSpace)
                        targets[i].localPosition = Vector3.Lerp(startPositions[i], step.targetPosition, t);
                    else
                        targets[i].position = Vector3.Lerp(startPositions[i], step.targetPosition, t);
                }
                yield return null;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] == null) continue;
                if (step.useLocalSpace)
                    targets[i].localPosition = step.targetPosition;
                else
                    targets[i].position = step.targetPosition;
            }

            onComplete?.Invoke();
        }
    }
}