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

       public ExperimentZone _zone;

        [Header("References")]
        [SerializeField] SessionManager _session;

       [SerializeField] bool _isInteractable = true;

        public static Clickable CurrentlyClicked;
        public bool IsInteractable => _isInteractable;

        public void SetInteractable(bool state)
        {
            _isInteractable = state;
        }

        // called by ClickInputController when this GO is clicked
        public void OnClicked(ExperimentZone zone)
        {
            CurrentlyClicked = this;
         //   Debug.Log("Clicked");
            if (!_isInteractable) return;
            zone.TryClick(this);
        }
    }
}
