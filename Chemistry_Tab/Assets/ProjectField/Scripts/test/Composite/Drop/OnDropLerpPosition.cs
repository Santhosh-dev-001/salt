using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Interface;
using SaltAnalysis.Data;

namespace SaltAnalysis.Interaction
{
    public class OnDropLerpPosition : MonoBehaviour, IDropEffect
    {
        [System.Serializable]
        public class PositionTarget
        {
            public Transform target;
            public Vector3 targetPosition;
            public bool useLocalSpace;
            public float duration = 1f;
        }

        [SerializeField] string[] _ids;
        [SerializeField] List<PositionTarget> _targets;

        public void Execute(InteractionStepData step)
        {
            if (!step.lerpPosition) return;
            if (!IdMatches()) return;

            foreach (var t in _targets)
                if (t.target != null)
                    StartCoroutine(LerpPosition(t));
        }

        IEnumerator LerpPosition(PositionTarget t)
        {
            float elapsed = 0f;

            if (t.useLocalSpace)
            {
                Vector3 start = t.target.localPosition;
                while (elapsed < t.duration)
                {
                    elapsed += Time.deltaTime;
                    t.target.localPosition = Vector3.Lerp(start, t.targetPosition,
                        elapsed / t.duration);
                    yield return null;
                }
                t.target.localPosition = t.targetPosition;
            }
            else
            {
                Vector3 start = t.target.position;
                while (elapsed < t.duration)
                {
                    elapsed += Time.deltaTime;
                    t.target.position = Vector3.Lerp(start, t.targetPosition,
                        elapsed / t.duration);
                    yield return null;
                }
                t.target.position = t.targetPosition;
            }
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