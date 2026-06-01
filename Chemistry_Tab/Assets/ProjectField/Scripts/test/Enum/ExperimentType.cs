using System;

namespace SaltAnalysis.Core
{
    public enum ExperimentType
    {
        PreliminaryExamination = 0,
        FlameTest = 1,
        GasEvolutionTest = 2,
        ConfirmatoryTest = 3
    }
}

namespace SaltAnalysis.Core
{
    public enum InteractionType
    {
        Drag,
        Click
    }
}

namespace SaltAnalysis.Core
{
    [Flags]
    public enum StepEffects
    {
        None = 0,
        Animation = 1,
        Activation = 2,
        Deactivation = 4,
        MaterialColor = 8,
        MaterialFloat = 16,
        ParticleColor = 32,
        LerpPosition = 64
    }
}