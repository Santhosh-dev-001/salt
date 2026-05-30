using UnityEngine;
using SaltAnalysis.Interface;

namespace SaltAnalysis.Reset
{
    public class ResettableParticle : MonoBehaviour, IResetComponent
    {
        ParticleSystem _ps;

        public void CaptureDefault()
        {
            _ps = GetComponent<ParticleSystem>();
        }

        public void Reset()
        {
            if (_ps == null) return;
            _ps.Stop();
            _ps.Clear();
        }
    }
}
