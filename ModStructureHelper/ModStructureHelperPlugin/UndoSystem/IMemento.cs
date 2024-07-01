using System.Collections;

namespace ModStructureHelperPlugin.UndoSystem;

public interface IMemento
{
    public IEnumerator Restore();
    // Used to synchronize undoing multiple actions that occured in the same frame
    public int SaveFrame { get; }
    public bool Invalid { get; }
}