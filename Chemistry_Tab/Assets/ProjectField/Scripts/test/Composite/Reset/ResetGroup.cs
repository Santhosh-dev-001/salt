using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Interface;

namespace SaltAnalysis.Reset
{
    public class ResetGroup : MonoBehaviour
    {
        List<IResetComponent> _components = new();

        void Awake()
        {
            GetComponents(_components);
            foreach (var c in _components)
                c.CaptureDefault();
        }

        public void ResetAll()
        {
            foreach (var c in _components)
                c.Reset();
        }
    }
}
