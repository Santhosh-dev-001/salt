using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Core;
using SaltAnalysis.Interface;
using SaltAnalysis.Data;

namespace SaltAnalysis.Interaction
{
    public class OnDropParticleColor : MonoBehaviour, IDropEffect
    {
        [System.Serializable]
        public class SaltEntry
        {
            public SaltType saltType;
            public string stringId;
            public List<ParticleSystem> targets = new();
        }

        [SerializeField] List<SaltEntry> _entries = new();

        Dictionary<(SaltType, string), List<ParticleSystem>> _map;

        void Awake()
        {
            _map = new();
            foreach (var e in _entries)
                _map[(e.saltType, e.stringId)] = e.targets;
        }

        public bool IsFlagged(InteractionStep step)
        {
            if (!step.stepEffects.HasFlag(StepEffects.ParticleColor)) return false;
            if (Draggable.CurrentlyDragged == null) return false;
            return _map.ContainsKey((Draggable.CurrentlyDragged.saltType, Draggable.CurrentlyDragged.stringId));
        }

        public void Execute(InteractionStep step, System.Action onComplete)
        {
            if (!IsFlagged(step)) return;

            var key = (Draggable.CurrentlyDragged.saltType, Draggable.CurrentlyDragged.stringId);
            if (!_map.TryGetValue(key, out var targets)) return;

            StartCoroutine(LerpAllParticles(targets, step, onComplete));
        }

        IEnumerator LerpAllParticles(List<ParticleSystem> targets, InteractionStep step, System.Action onComplete)
        {
            float elapsed = 0f;
            Color[] startColors = new Color[targets.Count];

            for (int i = 0; i < targets.Count; i++)
                if (targets[i] != null)
                    startColors[i] = targets[i].main.startColor.color;

            while (elapsed < step.particleColorDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / step.particleColorDuration;
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i] == null) continue;
                    var main = targets[i].main;
                    main.startColor = Color.Lerp(startColors[i], step.particleColor, t);
                }
                yield return null;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] == null) continue;
                var main = targets[i].main;
                main.startColor = step.particleColor;
            }

            onComplete?.Invoke();
        }
    }
}