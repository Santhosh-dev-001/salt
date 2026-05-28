using UnityEngine;
using SaltAnalysis.Core;

namespace SaltAnalysis.Data
{
    [CreateAssetMenu(fileName = "NewSaltData", menuName = "SaltAnalysis/Salt Data")]
    public class SaltAnalysisData : ScriptableObject
    {
        [Header("Identity")]
        public SaltType saltType;
        public string   saltName;
        public Sprite   saltSprite;
        public Color    saltColor = Color.white;

        [Header("Preliminary Examination")]
        public string physicalAppearance;
        public string colour;
        public string odour;
        public bool   isSoluble;
        public string solubilityNote;

        [Header("Flame Test")]
        public Color  flameColour;
        public string flameColourName;
        public bool   showsFlameColour;

        [Header("Gas Evolution Test")]
        public string reagentUsed;
        public string gasEvolved;
        public string gasColour;
        public string gasOdour;
        public string litmusEffect;
        public string gasInference;

        [Header("Confirmatory Test")]
        public string confirmatoryReagent;
        public string precipitateColour;
        public string confirmatoryObservation;
        public string confirmatoryInference;
    }
}
