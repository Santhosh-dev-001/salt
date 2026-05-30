using SaltAnalysis.Interaction;

namespace SaltAnalysis.Interface
{
    public interface IDropTarget
    {
        void OnReceiveDrop(Draggable draggable);
    }
}
