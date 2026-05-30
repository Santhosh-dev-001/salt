using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Interface;
using SaltAnalysis.Data;

namespace SaltAnalysis.Interaction
{
    public class OnDropParticleColor : MonoBehaviour, IDropEffect
    {
        [SerializeField] string[] _ids;
        [SerializeField] List<ParticleSystem> _targets;

        public void Execute(InteractionStepData step)
        {
            if (!step.changeParticleColor) return;
            if (!IdMatches()) return;

            foreach (var ps in _targets)
                if (ps != null)
                    StartCoroutine(LerpParticle(ps, step));
        }

        IEnumerator LerpParticle(ParticleSystem ps, InteractionStepData step)
        {
            ParticleSystem.MainModule main = ps.main;
            Color start = main.startColor.color;
            float elapsed = 0f;

            while (elapsed < step.particleColorDuration)
            {
                elapsed += Time.deltaTime;
                main.startColor = Color.Lerp(start, step.particleColor,
                    elapsed / step.particleColorDuration);
                yield return null;
            }

            main.startColor = step.particleColor;
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