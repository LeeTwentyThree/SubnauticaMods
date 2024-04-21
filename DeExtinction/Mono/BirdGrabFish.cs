using UnityEngine;

namespace DeExtinction.Mono;

public class BirdGrabFish : MonoBehaviour
{
    public Creature creature;
    public Transform fishParent;
    
    private GameObject _heldFish;
    private float _originalFishScale;

    public bool IsHoldingFish => _heldFish != null;

    private void OnTriggerEnter(Collider other)
    {
        if (_heldFish != null) return;
        var otherCreature = other.gameObject.GetComponentInParent<Creature>();
        if (otherCreature == null) return;
        if (IsCreatureValid(otherCreature))
            GrabFish(otherCreature);
    }

    private bool IsCreatureValid(Creature creature)
    {
        var ecoTarget = creature.gameObject.GetComponent<EcoTarget>();
        if (ecoTarget == null) return false;
        if (ecoTarget.type == EcoTargetType.SmallFish)
        {
            return true;
        }

        if (ecoTarget.type == EcoTargetType.MediumFish && creature.liveMixin.maxHealth < 50)
        {
            return true;
        }
        return false;
    }

    private void GrabFish(Creature prey)
    {
        _heldFish = prey.gameObject;
        _originalFishScale = _heldFish.transform.localScale.x;
        prey.GetComponent<Rigidbody>().isKinematic = true;
        prey.gameObject.AddComponent<FishBleedOut>().dealer = creature.gameObject;
        var fishTransform = prey.transform;
        fishTransform.parent = fishParent;
        fishTransform.localPosition = Vector3.zero;
        fishTransform.localRotation = Quaternion.identity;
        foreach (var collider in prey.GetComponentsInChildren<Collider>(true))
        {
            collider.enabled = false;
        }
        Invoke(nameof(Kill), 2);
        Invoke(nameof(Eat), 8);
        InvokeRepeating(nameof(Bite), 1f, 1f);
    }

    private void Bite()
    {
        if (_heldFish == null) return;
        _heldFish.transform.localScale = Vector3.one * Mathf.Clamp(_heldFish.transform.localScale.x - 0.1f * _originalFishScale, 0, _originalFishScale);
    }
    
    private void Kill()
    {
        if (_heldFish != null)
        {
            _heldFish.GetComponent<LiveMixin>().TakeDamage(60, transform.position, DamageType.Normal, creature.gameObject);
        }
    }

    private void Update()
    {
        if (_heldFish != null)
        {
            _heldFish.transform.parent = fishParent;
            var rb = _heldFish.GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = true;
            _heldFish.transform.localPosition = Vector3.zero;
        }
    }

    private void Eat()
    {
        Destroy(_heldFish);
        creature.Hunger.Value = 0;
    }
}