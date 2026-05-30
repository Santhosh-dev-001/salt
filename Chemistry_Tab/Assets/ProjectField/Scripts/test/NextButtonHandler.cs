using UnityEngine;
using SaltAnalysis.Data;

namespace SaltAnalysis.UI
{
    public class NextButtonHandler : MonoBehaviour
    {
        [SerializeField] SessionManager _session;

        // assign to Button OnClick() in Inspector
        public void OnNextPressed()
        {
            _session.IncrementStep();
        }
    }
}