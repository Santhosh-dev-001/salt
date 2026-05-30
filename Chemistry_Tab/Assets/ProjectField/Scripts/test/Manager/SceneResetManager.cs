using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Data;
using SaltAnalysis.Reset;

namespace SaltAnalysis.Core
{
    public class SceneResetManager : MonoBehaviour
    {
        [SerializeField] SessionManager    _session;
        [SerializeField] List<ResetGroup>  _groups;

        void OnEnable()  => _session.RegisterResetCallback(ResetAll);
        void OnDisable() => _session.RegisterResetCallback(null);

        public void ResetAll()
        {
            foreach (var g in _groups)
                if (g != null) g.ResetAll();
        }
    }
}
