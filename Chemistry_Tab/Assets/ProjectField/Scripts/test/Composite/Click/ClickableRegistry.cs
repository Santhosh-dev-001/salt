using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SaltAnalysis.Interaction
{
    public class ClickableRegistry : MonoBehaviour
    {
        [SerializeField] List<ClickableStepIndex> _entries = new();

        public void SetupForStep(int stepIndex)
        {
            
            foreach (var e in _entries)
            {
                if (e == null || e.Clickable == null) continue;
                bool match = e.StepIndex.Contains(stepIndex);
                e.Clickable.SetInteractable(match);
            }
        }

        public void SetAllInteractable(bool state)
        {
            foreach (var e in _entries)
                if (e != null && e.Clickable != null)
                    e.Clickable.SetInteractable(state);
        }
    }
}
