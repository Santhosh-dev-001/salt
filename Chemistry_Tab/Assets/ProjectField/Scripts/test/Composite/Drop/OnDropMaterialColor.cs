using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Interface;
using SaltAnalysis.Data;

namespace SaltAnalysis.Interaction
{
    public class OnDropMaterialColor : MonoBehaviour, IDropEffect
    {
        [SerializeField] string[] _ids;
        [SerializeField] List<Renderer> _targets;

        public void Execute(InteractionStepData step)
        {
            if (!step.changeMaterialColor) return;
            if (!IdMatches()) return;

            foreach (var r in _targets)
                if (r != null)
                    StartCoroutine(LerpColor(r, step));
        }

        IEnumerator LerpColor(Renderer r, InteractionStepData step)
        {
            Color start = r.material.GetColor(step.materialColorProperty);
            float elapsed = 0f;

            while (elapsed < step.materialColorDuration)
            {
                elapsed += Time.deltaTime;
                r.material.SetColor(step.materialColorProperty,
                    Color.Lerp(start, step.materialColor, elapsed / step.materialColorDuration));
                yield return null;
            }

            r.material.SetColor(step.materialColorProperty, step.materialColor);
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