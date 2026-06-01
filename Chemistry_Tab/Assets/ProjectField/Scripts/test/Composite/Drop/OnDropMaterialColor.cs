using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Core;
using SaltAnalysis.Interface;
using SaltAnalysis.Data;

namespace SaltAnalysis.Interaction
{
    public class OnDropMaterialColor : MonoBehaviour, IDropEffect
    {
        [System.Serializable]
        public class SaltEntry
        {
            public SaltType saltType;
            public string stringId;
            public List<Renderer> targets = new();
        }

        [SerializeField] List<SaltEntry> _entries = new();

        Dictionary<(SaltType, string), List<Renderer>> _map;

        void Awake()
        {
            _map = new();
            foreach (var e in _entries)
                _map[(e.saltType, e.stringId)] = e.targets;
        }

        public bool IsFlagged(InteractionStep step)
        {
            if (!step.stepEffects.HasFlag(StepEffects.MaterialColor)) return false;
            if (Draggable.CurrentlyDragged == null) return false;
            return _map.ContainsKey((Draggable.CurrentlyDragged.saltType, Draggable.CurrentlyDragged.stringId));
        }

        public void Execute(InteractionStep step, System.Action onComplete)
        {
            if (!IsFlagged(step)) return;

            var key = (Draggable.CurrentlyDragged.saltType, Draggable.CurrentlyDragged.stringId);
            if (!_map.TryGetValue(key, out var targets)) return;

            StartCoroutine(LerpAllColors(targets, step, onComplete));
        }

        IEnumerator LerpAllColors(List<Renderer> targets, InteractionStep step, System.Action onComplete)
        {
            float elapsed = 0f;
            Color[] startColors = new Color[targets.Count];

            for (int i = 0; i < targets.Count; i++)
                if (targets[i] != null)
                    startColors[i] = targets[i].material.GetColor(step.materialColorProperty);

            while (elapsed < step.materialColorDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / step.materialColorDuration;
                for (int i = 0; i < targets.Count; i++)
                    if (targets[i] != null)
                        targets[i].material.SetColor(step.materialColorProperty,
                            Color.Lerp(startColors[i], step.materialColor, t));
                yield return null;
            }

            for (int i = 0; i < targets.Count; i++)
                if (targets[i] != null)
                    targets[i].material.SetColor(step.materialColorProperty, step.materialColor);

            onComplete?.Invoke();
        }
    }
}