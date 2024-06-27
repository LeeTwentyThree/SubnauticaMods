namespace ModStructureHelperPlugin.UndoSystem;

public interface IOriginator
{
    public IMemento GetSnapshot();
}