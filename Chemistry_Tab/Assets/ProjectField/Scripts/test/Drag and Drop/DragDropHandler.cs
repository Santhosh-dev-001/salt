using UnityEngine;
using SaltAnalysis.Interface;

namespace SaltAnalysis.Interaction
{
    public class DragDropHandler : MonoBehaviour
    {
        public void OnDrop(SaltDraggable draggable, IExperimentZone zone)
        {
            if (draggable == null || zone == null) return;

            zone.Execute(draggable.SaltType);
        }
    }
}
