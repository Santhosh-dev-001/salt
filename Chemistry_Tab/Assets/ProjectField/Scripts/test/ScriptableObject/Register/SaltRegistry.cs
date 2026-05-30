using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Core;

namespace SaltAnalysis.Data
{
    [CreateAssetMenu(fileName = "SaltRegistry", menuName = "SaltAnalysis/Salt Registry")]
    public class SaltRegistry : ScriptableObject
    {
        [SerializeField] List<SaltAnalysisData> _salts = new();

        public SaltAnalysisData Get(SaltType type)
        {
            return _salts.Find(d => d.saltType == type);
        }

        public IReadOnlyList<SaltAnalysisData> All => _salts;
    }
}
