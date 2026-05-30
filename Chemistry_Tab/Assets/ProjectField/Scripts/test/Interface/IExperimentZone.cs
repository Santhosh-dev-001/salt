using SaltAnalysis.Core;
using SaltAnalysis.Data;

namespace SaltAnalysis.Interface
{
    public interface IExperimentZone
    {
        ExperimentType ExperimentType { get; }
        bool IsStepAllowed(SessionManager session);
        void Execute(SaltType salt);
    }
}