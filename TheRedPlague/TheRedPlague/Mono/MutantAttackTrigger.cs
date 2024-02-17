using System;
using UnityEngine;

namespace TheRedPlague.Mono;

public class MutantAttackTrigger : MonoBehaviour
{
    public string prefabFileName;
    public bool heavilyMutated;

    private GameObject _model;

    private void Start()
    {
        _model = transform.parent.GetChild(0)?.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GetTarget(other).GetComponent<Player>() != null)
        {
            DeathScare.PlayMutantDeathScare(prefabFileName, heavilyMutated);
            if (_model) _model.SetActive(false);
            Invoke(nameof(ReEnableModel), 5);
        }
    }
    
    private GameObject GetTarget(Collider collider)
    {
        var other = collider.gameObject;
        if (other.GetComponent<LiveMixin>() == null && collider.attachedRigidbody != null)
        {
            other = collider.attachedRigidbody.gameObject;
        }
        return other;
    }

    private void ReEnableModel()
    {
        if (_model) _model.SetActive(true);
    }
}