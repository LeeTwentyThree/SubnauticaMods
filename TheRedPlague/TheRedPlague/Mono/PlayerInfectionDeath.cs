using System;
using System.Collections;
using Nautilus.Utility;
using Story;
using UnityEngine;

namespace TheRedPlague.Mono;

public class PlayerInfectionDeath : MonoBehaviour
{
    public static PlayerInfectionDeath main;

    private static bool _creatingPrefab;

    private DeathCountdownUI _ui;

    private bool _justLoaded = true;

    private void Awake()
    {
        main = this;
        _ui = DeathCountdownUI.Create();
    }

    public static void SetDeathTimer(float timer)
    {
        if (main == null)
        {
            if (!_creatingPrefab)
                UWE.CoroutineHost.StartCoroutine(CreateAndSetTimer(timer));
            return;
        }
        main.SetTimer(timer);
        main._ui.gameObject.SetActive(true);
    }

    private static IEnumerator CreateAndSetTimer(float timer)
    {
        var task = CraftData.GetPrefabForTechTypeAsync(ModPrefabs.InfectionTimerInfo.TechType);
        yield return task;
        var go = Instantiate(task.GetResult());
        go.SetActive(true);
        LargeWorldStreamer.main.cellManager.RegisterGlobalEntity(go);
        go.GetComponent<PlayerInfectionDeath>().SetTimer(timer);
        main._ui.gameObject.SetActive(true);
    }

    public float GetRemainingTime() => transform.localScale.y;

    private void SetTimer(float timer)
    {
        transform.localScale = new Vector3(1, timer, 1);
    }

    private void Update()
    {
        var time = Mathf.Max(0, GetRemainingTime() - Time.deltaTime);
        SetTimer(time);
        _ui.UpdateDisplay(Mathf.RoundToInt(time));
        if (time > 0 && _justLoaded)
        {
            _justLoaded = false;
        }
        
        _ui.gameObject.SetActive(time > 0);

        if (time == 0 && !_justLoaded)
        {
            var infectDamage = Player.main.GetComponent<PlayerInfectDamage>();
            if (!infectDamage)
            {
                Player.main.gameObject.EnsureComponent<PlayerInfectDamage>();
                var infectVisuals = Player.main.gameObject.EnsureComponent<InfectAnything>();
                infectVisuals.infectionHeightStrength = 0.1f;
                Utils.PlayFMODAsset(AudioUtils.GetFmodAsset("DieFromInfection"), Player.main.transform.position);
                _ui.gameObject.SetActive(false);
            }
        }
    }

    public static void PlayCinematic(string key)
    {
        if (key == StoryUtils.PlagueHeartGoal.key)
        {
            SetDeathTimer(15 * 60);
            Inventory.main.SecureItems(false);
        }
        if (key == StoryUtils.ForceFieldLaserDisabled.key && main != null)
        {
            Destroy(main.gameObject);
        }
    }

    private void OnDestroy()
    {
        _ui.gameObject.SetActive(false);
    }
}