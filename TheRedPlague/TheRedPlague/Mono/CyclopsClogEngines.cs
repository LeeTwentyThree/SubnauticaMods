using System.Collections;
using ECCLibrary;
using Nautilus.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheRedPlague.Mono;

public class CyclopsClogEngines : MonoBehaviour
{
    public bool plagueCyclops;
    public SubRoot sub;

    private float _timeCanClogAgain;

    private GameObject _bloodPrefab;
    private GameObject _propellerObstruction;

    private float _timeTryAgain;

    private LiveMixinData _lmData;

    private static readonly FMODAsset ClogVoiceLine = AudioUtils.GetFmodAsset("PlagueCyclopsPropellerObstruction");

    private void Awake()
    {
        _lmData = CreatureDataUtils.CreateLiveMixinData(20);
        _lmData.broadcastKillOnDeath = true;
    }

    private IEnumerator Start()
    {
        var peeperTask = CraftData.GetPrefabForTechTypeAsync(TechType.Peeper);
        yield return peeperTask;
        _bloodPrefab = peeperTask.GetResult().GetComponent<LiveMixin>().data.damageEffect;
    }

    private void Update()
    {
        if (plagueCyclops) return;
        if (Time.time < _timeCanClogAgain) return;
        if (Time.time < _timeTryAgain) return;
        if (_propellerObstruction != null) return;
        _timeTryAgain = Time.time + 1;
        if (Player.main.GetCurrentSub() != sub) return;
        var biomeString = Player.main.GetBiomeString();
        if (!biomeString.Contains("dunes")) return;
        SpawnPropellerObstruction();
    }

    private void LateUpdate()
    {
        if (_propellerObstruction != null)
        {
            sub.rigidbody.isKinematic = true;
        }
    }

    private void SpawnPropellerObstruction()
    {
        var colliders = gameObject.GetComponentsInChildren<Collider>();
        _propellerObstruction = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("CyclopsPropellorObstructionPrefab"), transform);
        _propellerObstruction.transform.localPosition = new Vector3(0.2f, 1.5f, 25);
        _propellerObstruction.transform.localScale = Vector3.one * 10;
        MaterialUtils.ApplySNShaders(_propellerObstruction, 4f, 2f);
        _propellerObstruction.AddComponent<SkyApplier>().renderers = _propellerObstruction.GetComponentsInChildren<Renderer>();
        var rb = _propellerObstruction.AddComponent<Rigidbody>();
        rb.mass = 2000;
        rb.isKinematic = true;
        var lm = _propellerObstruction.AddComponent<LiveMixin>();
        lm.data = _lmData;
        lm.health = _lmData.maxHealth;
        _timeCanClogAgain = Time.time + 30;
        Utils.PlayFMODAsset(ClogVoiceLine, transform.position);
        Subtitles.Add("PlagueCyclopsPropellerObstruction");
        var obstructionCollider = _propellerObstruction.GetComponentInChildren<Collider>();
        foreach (var collider in colliders)
        {
            Physics.IgnoreCollision(obstructionCollider, collider);
        }

        _propellerObstruction.AddComponent<ObstructionDeconstructionListener>().clog = this;
    }

    public void KillPropellerObstruction()
    {
        if (_propellerObstruction == null) return;
        SpawnBloodFX();
        Destroy(_propellerObstruction, 1f);
        _timeCanClogAgain = Time.time + 30;
        sub.rigidbody.isKinematic = false;
    }

    public void OnKill()
    {
        KillPropellerObstruction();
    }

    private void SpawnBloodFX()
    {
        if (_bloodPrefab == null) return;
        for (int i = 0; i < 14; i++)
        {
            var blood = Instantiate(_bloodPrefab, _propellerObstruction.transform.position + Random.insideUnitSphere * 4, Random.rotation);
            var renderers = blood.GetComponentsInChildren<Renderer>();
            foreach (var r in renderers)
            {
                var materials = r.materials;
                foreach (var material in materials)
                {
                    material.SetColor("_Color", Color.red * 0.35f);
                    material.SetColor("_GlowColor", Color.black);
                }

                r.materials = materials;
            }

            foreach (var ps in blood.GetComponentsInChildren<ParticleSystem>())
            {
                var main = ps.main;
                main.startSizeMultiplier *= 3;
            }
        }
    }
}