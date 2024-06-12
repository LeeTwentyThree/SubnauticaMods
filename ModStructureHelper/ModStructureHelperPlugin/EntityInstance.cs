using ModStructureFormat;
using UnityEngine;

namespace ModStructureHelperPlugin;

public class EntityInstance : MonoBehaviour
{
    public ManagedEntity ManagedEntity;
    
    // Non-changeable properties
    public Entity.CellLevel CellLevel { get; private set; }
    public string ClassId { get; private set; }
    public string Id { get; private set; }
    
    // Changeable properties
    public Entity.Priority Priority { get; set; }
    
    private void Awake()
    {
        var lwe = gameObject.GetComponent<LargeWorldEntity>();
        if (lwe)
            CellLevel = (Entity.CellLevel) (int) lwe.cellLevel;
        else
            CellLevel = Entity.CellLevel.Unknown;
        var prefabIdentifier = gameObject.GetComponent<PrefabIdentifier>();
        if (prefabIdentifier)
        {
            ClassId = prefabIdentifier.ClassId;
            Id = prefabIdentifier.Id;
        }

        StructureInstance.OnStructureInstanceUpdated += OnStructureInstanceUpdated;

        // DestroyImmediate(lwe);
    }
    
    public Entity GetEntityDataStruct()
    {
        return new Entity(ClassId, Id, transform.position, transform.rotation, transform.localScale, CellLevel, Priority);
    }

    private void OnDestroy()
    {
        ManagedEntity.RemoveCurrentEntityInstance();
        StructureInstance.OnStructureInstanceUpdated -= OnStructureInstanceUpdated;
    }

    private void OnStructureInstanceUpdated(StructureInstance instance)
    {
        if (instance == null)
            Destroy(this);
    }
}