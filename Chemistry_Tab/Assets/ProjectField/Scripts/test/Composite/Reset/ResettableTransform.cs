using UnityEngine;
using SaltAnalysis.Interface;

namespace SaltAnalysis.Reset
{
    public class ResettableTransform : MonoBehaviour, IResetComponent
    {
        Vector3    _startPos;
        Quaternion _startRot;

        public void CaptureDefault()
        {
            _startPos = transform.position;
            _startRot = transform.rotation;
        }

        public void Reset()
        {
            transform.position = _startPos;
            transform.rotation = _startRot;
        }
    }
}
