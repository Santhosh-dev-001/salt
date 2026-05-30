using UnityEngine;
using SaltAnalysis.Interface;

namespace SaltAnalysis.Reset
{
    public class ResettableMaterialFloat : MonoBehaviour, IResetComponent
    {
        [SerializeField] string _property = "_Metallic";

        Renderer _renderer;
        float    _startValue;

        public void CaptureDefault()
        {
            _renderer   = GetComponent<Renderer>();
            _startValue = _renderer.material.GetFloat(_property);
        }

        public void Reset()
        {
            if (_renderer != null)
                _renderer.material.SetFloat(_property, _startValue);
        }
    }
}
