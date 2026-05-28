using SaltAnalysis.Core;

namespace SaltAnalysis.Interface
{
    public interface IExperimentZone
    {
        ExperimentType ExperimentType { get; }

        void Execute(SaltType salt);
    }
}
