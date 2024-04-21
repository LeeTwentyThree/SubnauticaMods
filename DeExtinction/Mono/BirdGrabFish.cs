using UnityEngine;

namespace DeExtinction.Mono;

public class BirdGrabFish : MonoBehaviour
{
    public Transform fishParent;
    private GameObject _heldFish;
    private float _originalFishScale;

    public bool IsHoldingFish => _heldFish != null;

    private void OnTriggerEnter(Collider other)
    {
        if (_heldFish != null) return;
        var creature = other.gameObject.GetComponentInParent<Creature>();
        if (creature == null) return;
        if (IsCreatureValid(creature))
            GrabFish(creature);
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

    private void GrabFish(Creature fish)
    {
        _heldFish = fish.gameObject;
        _originalFishScale = _heldFish.transform.localScale.x;
        fish.GetComponent<Rigidbody>().isKinematic = true;
        fish.gameObject.AddComponent<FishBleedOut>();
        var fishTransform = fish.transform;
        fishTransform.parent = fishParent;
        fishTransform.localPosition = Vector3.zero;
        fishTransform.localRotation = Quaternion.identity;
        foreach (var collider in fish.GetComponentsInChildren<Collider>(true))
        {
            collider.enabled = false;
        }
        Invoke(nameof(Kill), 2);
        Invoke(nameof(Eat), 6);
        InvokeRepeating(nameof(Bite), 1f, 1f);
    }

    private void Bite()
    {
        if (_heldFish == null) return;
        _heldFish.transform.localScale = Vector3.one * Mathf.Clamp(_heldFish.transform.localScale.x - 0.2f * _originalFishScale, 0, _originalFishScale);
    }
    
    private void Kill()
    {
        if (_heldFish != null)
        {
            _heldFish.GetComponent<LiveMixin>().TakeDamage(60, transform.position);
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
    }
}