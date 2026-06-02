using UnityEngine;
using SaltAnalysis.Core;
using SaltAnalysis.Data;

namespace SaltAnalysis.Interaction
{
    public class Clickable : MonoBehaviour
    {
        [Header("Identity")]
        [SerializeField] public SaltType saltType;
        [SerializeField] public string   stringId;

        [Header("References")]
        [SerializeField] SessionManager _session;

        bool _isInteractable = true;

        public bool IsInteractable => _isInteractable;

        public void SetInteractable(bool state)
        {
            _isInteractable = state;
        }

        // called by ClickInputController when this GO is clicked
        public void OnClicked(ExperimentZone zone)
        {
            if (!_isInteractable) return;
            zone.TryClick(this);
        }
    }
}
