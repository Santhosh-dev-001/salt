using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Interface;
using SaltAnalysis.Data;

namespace SaltAnalysis.Interaction
{
    public class OnDropActivation : MonoBehaviour, IDropEffect
    {
        [SerializeField] string[] _ids;
        [SerializeField] List<GameObject> _targets;

        public void Execute(InteractionStepData step)
        {
            if (!step.activateObjects) return;
            if (!IdMatches()) return;

            foreach (var go in _targets)
                if (go != null) go.SetActive(true);
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