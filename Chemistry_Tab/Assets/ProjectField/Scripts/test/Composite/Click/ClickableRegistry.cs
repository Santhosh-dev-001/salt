using System.Collections.Generic;
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
                e.Clickable.SetInteractable(e.StepIndex == stepIndex);
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
