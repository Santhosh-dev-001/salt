using System.Collections.Generic;
using UnityEngine;

namespace SaltAnalysis.Interaction
{
    public class DraggableRegistry : MonoBehaviour
    {
        [SerializeField] List<DraggableStepIndex> _entries = new();

        public void SetupForStep(int stepIndex)
        {
            foreach (var e in _entries)
            {
                if (e == null || e.Draggable == null) continue;
                bool match = e.StepIndex.Contains(stepIndex);
                e.Draggable.SetInteractable(match);
            }
        }

        public void SetAllInteractable(bool state)
        {
            foreach (var e in _entries)
                if (e != null && e.Draggable != null)
                    e.Draggable.SetInteractable(state);
        }
    }
}