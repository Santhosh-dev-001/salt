using UnityEngine;
using SaltAnalysis.Interface;

namespace SaltAnalysis.Reset
{
    public class ResettableActiveState : MonoBehaviour, IResetComponent
    {
        bool _startState;

        public void CaptureDefault()
        {
            _startState = gameObject.activeSelf;
        }

        public void Reset()
        {
            gameObject.SetActive(_startState);
        }
    }
}
