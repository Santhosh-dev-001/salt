using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Interface;
using SaltAnalysis.Data;

namespace SaltAnalysis.Interaction
{
    public class OnDropMaterialFloat : MonoBehaviour, IDropEffect
    {
        [SerializeField] string[] _ids;
        [SerializeField] List<Renderer> _targets;

        public void Execute(InteractionStepData step)
        {
            if (!step.changeMaterialFloat) return;
            if (!IdMatches()) return;

            foreach (var r in _targets)
                if (r != null)
                    StartCoroutine(LerpFloat(r, step));
        }

        IEnumerator LerpFloat(Renderer r, InteractionStepData step)
        {
            float start = r.material.GetFloat(step.materialFloatProperty);
            float elapsed = 0f;

            while (elapsed < step.materialFloatDuration)
            {
                elapsed += Time.deltaTime;
                r.material.SetFloat(step.materialFloatProperty,
                    Mathf.Lerp(start, step.materialFloatValue, elapsed / step.materialFloatDuration));
                yield return null;
            }

            r.material.SetFloat(step.materialFloatProperty, step.materialFloatValue);
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