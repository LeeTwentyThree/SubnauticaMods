using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono;

public class MutantAttackTrigger : MonoBehaviour
{
    public string prefabFileName;
    public bool heavilyMutated;
    public float damage;
    public float instantKillChance = 0.1f;
    public float attackDelay = 1f;

    private GameObject _model;
    private float _timeCanAttackAgain;
    private static FMODAsset _zombieBiteSound = AudioUtils.GetFmodAsset("ZombieBite");

    private DisableRigidbodyWhileOnScreen _disableRigidbodyWhileOnScreen;

    private void Start()
    {
        _model = transform.parent.GetChild(0)?.gameObject;
        if (heavilyMutated)
            _disableRigidbodyWhileOnScreen = GetComponentInParent<DisableRigidbodyWhileOnScreen>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time < _timeCanAttackAgain) return;
        var player = GetTarget(other).GetComponent<Player>();
        if (player == null)
        {
            return;
        }
        DamagePlayer(player);
        _timeCanAttackAgain = Time.time + attackDelay;
    }

    private void DamagePlayer(Player player)
    {
        var calculatedDamage = DamageSystem.CalculateDamage(damage, DamageType.Normal, player.gameObject);
        if (calculatedDamage >= player.liveMixin.health)
        {
            JumpScare();
            return;
        }

        if (Random.value < instantKillChance)
        {
            JumpScare();
        }
        else
        {
            player.liveMixin.TakeDamage(calculatedDamage, transform.position);
            Utils.PlayFMODAsset(_zombieBiteSound, transform.position);
            if (_disableRigidbodyWhileOnScreen)
                _disableRigidbodyWhileOnScreen.UnfreezeForDuration(3);
        }
    }

    private void JumpScare()
    {
        DeathScare.PlayMutantDeathScare(prefabFileName, heavilyMutated);
        if (_model) _model.SetActive(false);
        Invoke(nameof(ReEnableModel), 5);
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