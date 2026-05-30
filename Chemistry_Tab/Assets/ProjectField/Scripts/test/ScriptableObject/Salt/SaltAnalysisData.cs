using UnityEngine;
using System.Collections.Generic;
using SaltAnalysis.Core;

namespace SaltAnalysis.Data
{
    [CreateAssetMenu(fileName = "NewSaltData", menuName = "SaltAnalysis/Salt Data")]
    public class SaltAnalysisData : ScriptableObject
    {
        [Header("Identity")]
        public SaltType saltType;
        public string saltName;

        [Header("Experiments")]
        public ExperimentEffectData preliminary;
        public ExperimentEffectData flameTest;
        public ExperimentEffectData gasEvolution;
        public ExperimentEffectData confirmatory;

        public ExperimentEffectData GetEffectData(ExperimentType type)
        {
            switch (type)
            {
                case ExperimentType.PreliminaryExamination: return preliminary;
                case ExperimentType.FlameTest: return flameTest;
                case ExperimentType.GasEvolutionTest: return gasEvolution;
                case ExperimentType.ConfirmatoryTest: return confirmatory;
                default: return null;
            }
        }
    }

    [System.Serializable]
    public class ExperimentEffectData
    {
        public List<InteractionStepData> steps;
    }

    [System.Serializable]
    public class InteractionStepData
    {
        [Header("Step Identity")]
        public string draggableId;
        public string dropTargetId;

        [Header("Effects — toggle what this step does")]
        public bool playAnimation;
        public bool activateObjects;
        public bool deactivateObjects;
        public bool changeMaterialColor;
        public bool changeMaterialFloat;
        public bool changeParticleColor;
        public bool lerpPosition;

        [Header("Animation Data")]
        public Animator animatorController;
        public string animationClip;

        [Header("Material Color Data")]
        public string materialColorProperty = "_Color";
        public Color materialColor = Color.white;
        public float materialColorDuration = 1f;

        [Header("Material Float Data")]
        public string materialFloatProperty = "_Metallic";
        public float materialFloatValue;
        public float materialFloatDuration = 1f;

        [Header("Particle Color Data")]
        public Color particleColor = Color.white;
        public float particleColorDuration = 1f;
    }
}