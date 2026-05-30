using UnityEngine;
using SaltAnalysis.Interface;

namespace SaltAnalysis.Reset
{
    public class ResettableMaterialColor : MonoBehaviour, IResetComponent
    {
        [SerializeField] string _property = "_Color";

        Renderer _renderer;
        Color    _startColor;

        public void CaptureDefault()
        {
            _renderer   = GetComponent<Renderer>();
            _startColor = _renderer.material.GetColor(_property);
        }

        public void Reset()
        {
            if (_renderer != null)
                _renderer.material.SetColor(_property, _startColor);
        }
    }
}
