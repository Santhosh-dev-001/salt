using UnityEngine;
using SaltAnalysis.Data;

namespace SaltAnalysis.UI
{
    public class NextButtonHandler : MonoBehaviour
    {
        [SerializeField] SessionManager _session;

        public void OnNextPressed()
        {
            _session.IncrementStep();
        }
    }
}