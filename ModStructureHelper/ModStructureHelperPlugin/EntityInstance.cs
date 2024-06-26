using System.Linq;
using ModStructureFormat;
using ModStructureHelperPlugin.Mono;
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

    private Rigidbody _rigidbody;
    
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

        StructureInstance.OnStructureInstanceChanged += OnStructureInstanceChanged;

        AccountForLackOfCollisions();

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody) _rigidbody.isKinematic = true;

        var liveMixin = GetComponent<LiveMixin>();
        if (liveMixin) liveMixin.invincible = true;

        // DestroyImmediate(lwe);
    }

    private void AccountForLackOfCollisions()
    {
        if (HasSolidCollisions()) return;
        var collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = 1;
        var selectionCollider = gameObject.AddComponent<EnableColliderForSelection>();
        selectionCollider.managedCollider = collider;
    }

    private bool HasSolidCollisions()
    {
        if (GetComponentInChildren<Collider>() == null)
        {
            return false;
        }

        return true;
        // we COULD discount triggers but meh
        // return GetComponentsInChildren<Collider>().Any(collider => !collider.isTrigger);
    }
    
    public Entity GetEntityDataStruct()
    {
        return new Entity(ClassId, Id, transform.position, transform.rotation, transform.localScale, CellLevel, Priority);
    }

    private void Update()
    {
        if (_rigidbody) _rigidbody.isKinematic = true;
    }

    private void OnDestroy()
    {
        ManagedEntity.RemoveAssignedEntityInstance();
        StructureInstance.OnStructureInstanceChanged -= OnStructureInstanceChanged;
    }

    private void OnStructureInstanceChanged(StructureInstance instance)
    {
        if (instance == null)
            Destroy(this);
    }
}