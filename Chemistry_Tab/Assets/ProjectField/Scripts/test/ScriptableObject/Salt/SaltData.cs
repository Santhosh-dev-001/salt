using System.Collections.Generic;
using UnityEngine;
using SaltAnalysis.Core;

namespace SaltAnalysis.Data
{
    [CreateAssetMenu(fileName = "NewSaltData", menuName = "SaltAnalysis/Salt Data")]
    public class SaltData: ScriptableObject
    {
        [Header("Identity")]
        public SaltType saltType;

        [Header("Interaction Steps")]
        public List<InteractionStep> steps = new();

        public InteractionStep GetStep(int index)
        {
            if (index < 0 || index >= steps.Count) return null;
            return steps[index];
        }

        public int StepCount => steps.Count;
    }
}


namespace SaltAnalysis.Data
{
    [System.Serializable]
    public class InteractionStep
    {
        [Header("Step Identity")]
        public InteractionType interactionType;
        public int stepCount;
        public StepEffects stepEffects;

        [Header("Animation")]
        public string animationClip;

        [Header("Material Color")]
        public string materialColorProperty = "_Color";
        public Color materialColor = Color.white;
        public float materialColorDuration = 1f;

        [Header("Material Float")]
        public string materialFloatProperty = "_Metallic";
        public float materialFloatValue;
        public float materialFloatDuration = 1f;

        [Header("Particle Color")]
        public Color particleColor = Color.white;
        public float particleColorDuration = 1f;

        [Header("Lerp Position")]
        public Vector3 targetPosition;
        public bool useLocalSpace;
        public float lerpPositionDuration = 1f;
    }
}
