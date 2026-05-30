using SaltAnalysis.Data;

namespace SaltAnalysis.Interface
{
    public interface IDropEffect
    {
        void Execute(InteractionStepData step);
    }
}