using System.Collections;

namespace ModStructureHelperPlugin.UndoSystem;

public readonly struct AddEntityMemento : IMemento
{
    private string Id { get; }
    public int SaveFrame { get; }
    public bool Invalid => false;

    public AddEntityMemento(string id, int saveFrame)
    {
        Id = id;
        SaveFrame = saveFrame;
    }

    public IEnumerator Restore()
    {
        foreach (var entity in StructureInstance.Main.GetAllManagedEntities())
        {
            if (entity.Id == Id)
            {
                StructureInstance.Main.DeleteEntity(entity.EntityInstance.ManagedEntity, false);
                yield break;
            }
        }
    }
}