using SaltAnalysis.Data;

namespace SaltAnalysis.Interface
{
    public interface IClickEffect
    {
        bool IsFlagged(InteractionStep step);
        void Execute(InteractionStep step, System.Action onComplete);
    }
}
