using ModStructureFormat;
using UnityEngine;

namespace ModStructureHelperPlugin;

public class EntityInstance : MonoBehaviour
{
    private Entity.CellLevel _cellLevel;
    private string _classId;
    private string _id;
    public Entity.Priority priority;
    
    private void Awake()
    {
        var lwe = gameObject.GetComponent<LargeWorldEntity>();
        if (lwe)
            _cellLevel = (Entity.CellLevel) (int) lwe.cellLevel;
        else
            _cellLevel = Entity.CellLevel.Unknown;
        var prefabIdentifier = gameObject.GetComponent<PrefabIdentifier>();
        if (prefabIdentifier)
        {
            _classId = prefabIdentifier.ClassId;
            _id = prefabIdentifier.Id;
        }

        DestroyImmediate(lwe);
    }
    
    public Entity GetEntityDataStruct()
    {
        return new Entity(_classId, _id, transform.position, transform.eulerAngles, transform.localScale, _cellLevel, priority);
    }
}