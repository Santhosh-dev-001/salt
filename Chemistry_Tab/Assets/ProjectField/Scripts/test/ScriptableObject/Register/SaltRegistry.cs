using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Core;

namespace SaltAnalysis.Data
{
    [CreateAssetMenu(fileName = "SaltRegistry", menuName = "SaltAnalysis/Salt Registry")]
    public class SaltRegistry : ScriptableObject
    {
        [SerializeField] List<SaltData> _salts = new();

        public SaltData Get(SaltType type)
        {
            return _salts.Find(d => d.saltType == type);
        }

        public IReadOnlyList<SaltData> All => _salts;
    }
}
