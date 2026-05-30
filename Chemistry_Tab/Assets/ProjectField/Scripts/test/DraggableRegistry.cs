using System.Collections.Generic;
using UnityEngine;

namespace SaltAnalysis.Interaction
{
    public class DraggableRegistry : MonoBehaviour
    {
        [SerializeField] List<Draggable> _draggables;

        public void SetInteractable(string id, bool state)
        {
            foreach (var d in _draggables)
                if (d != null && d.Id == id)
                    d.SetInteractable(state);
        }

        public void SetAllInteractable(bool state)
        {
            foreach (var d in _draggables)
                if (d != null)
                    d.SetInteractable(state);
        }
    }
}