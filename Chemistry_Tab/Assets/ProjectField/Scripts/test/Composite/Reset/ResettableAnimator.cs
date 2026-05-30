using UnityEngine;
using SaltAnalysis.Interface;

namespace SaltAnalysis.Reset
{
    public class ResettableAnimator : MonoBehaviour, IResetComponent
    {
        Animator _animator;

        public void CaptureDefault()
        {
            _animator = GetComponent<Animator>();
        }

        public void Reset()
        {
            if (_animator == null) return;
            _animator.Rebind();
            _animator.Update(0f);
        }
    }
}
