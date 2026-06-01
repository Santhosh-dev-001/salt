using SaltAnalysis.Data;

namespace SaltAnalysis.Interface
{
    public interface IDropEffect
    {
        bool IsFlagged(InteractionStep step);
        void Execute(InteractionStep step, System.Action onComplete);
    }
}