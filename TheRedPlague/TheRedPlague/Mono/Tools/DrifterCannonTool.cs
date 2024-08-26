using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.Tools;

public class DrifterCannonTool : PlayerTool
{
    public override string animToolName => "flashlight";

    public Animator animator;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public Renderer glowRenderer;
    
    public float fireDelay = 1f;
    public float fireDelayNoAmmo = 2f;
    public float launchVelocity = 45f;

    public float maxGlowStrength = 1f;
    public float maxGlowStrengthNight = 1f;

    public float healthConsumption = 10f;

    private float _timeCanFireAgain;

    private float _timeUpdateMaterialsAgain;

    private static readonly FMODAsset FireSound = AudioUtils.GetFmodAsset("DrifterCannonFire");
    private static readonly FMODAsset FireNoAmmoSound = AudioUtils.GetFmodAsset("DrifterCannonFireNoAmmo");
    
    public override void OnToolUseAnim(GUIHand hand)
    {
        if (Time.time < _timeCanFireAgain) return;
        if (!HasAmmo())
        {
            _timeCanFireAgain = Time.time + fireDelayNoAmmo;
            animator.SetTrigger("fire_noammo");
            Utils.PlayFMODAsset(FireNoAmmoSound, transform.position);
            return;
        }
        _timeCanFireAgain = Time.time + fireDelay;
        animator.SetTrigger("fire");
        ExpendAmmo();
        Utils.PlayFMODAsset(FireSound, transform.position);
        SpawnProjectile();
    }

    private void SpawnProjectile()
    {
        var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position + MainCamera.camera.transform.forward * 2,
            Quaternion.identity);
        projectile.transform.up = MainCamera.camera.transform.rotation * Vector3.forward;
        projectile.SetActive(true);
        var rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = usingPlayer.rigidBody.velocity;
        rb.AddForce(MainCamera.camera.transform.forward * launchVelocity, ForceMode.VelocityChange);
        projectile.AddComponent<DrifterCannonProjectile>();
    }

    private bool HasAmmo()
    {
        return DamageSystem.CalculateDamage(healthConsumption, DamageType.Normal, usingPlayer.gameObject) <
               usingPlayer.liveMixin.health;
    }

    private void ExpendAmmo()
    {
        usingPlayer.liveMixin.TakeDamage(healthConsumption);
        UpdateMaterials();
    }

    private void Update()
    {
        if (Time.time > _timeUpdateMaterialsAgain)
        {
            if (usingPlayer != null)
                UpdateMaterials();
            _timeUpdateMaterialsAgain = Time.time + 0.5f;
        }
    }

    private void UpdateMaterials()
    {
        var glowStrengthScale = usingPlayer.liveMixin.health / usingPlayer.liveMixin.maxHealth;
        glowRenderer.material.SetFloat(ShaderPropertyID._GlowStrength, maxGlowStrength * glowStrengthScale);
        glowRenderer.material.SetFloat(ShaderPropertyID._GlowStrengthNight, maxGlowStrengthNight * glowStrengthScale);
    }
}