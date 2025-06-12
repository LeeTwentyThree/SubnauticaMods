using System;
using System.Collections;
using System.Collections.Generic;
using Nautilus.Utility;
using PdaUpgradeCards.Data;
using UnityEngine;

namespace PdaUpgradeCards.MonoBehaviours.Upgrades;

public abstract class PocketDimensionUpgrade : UpgradeChipBase
{
    private const float BufferDuration = 0.5f;
    private static readonly FMODAsset TeleportSound = AudioUtils.GetFmodAsset("event:/env/use_teleporter_use_loop");

    private static readonly List<PocketDimensionUpgrade> All = new();
    private static PocketDimensionUpgrade _currentDimensionUpgrade;

    protected abstract TechType DimensionTechType { get; }
    protected abstract int DimensionTier { get; }
    protected abstract Vector3 WorldLocation { get; }

    private static PocketDimensionSub _activeSub;
    private static bool _bufferTimeActive;
    private static bool _loadingPocketDimensionPrefab;
    private static bool _cancelingLoading;
    private static bool _teleporting;
    private static bool _teleportationComplete;

    private static Coroutine _setUpCoroutine;
    private static Coroutine _teleportCoroutine;
    
    private void OnEnable()
    {
        All.Add(this);
        UpdateActiveDimension();
    }

    private void OnDisable()
    {
        if (All.Remove(this))
            UpdateActiveDimension();
    }

    private static void UpdateActiveDimension()
    {
        if (All.Count == 0)
        {
            _currentDimensionUpgrade = null;
            DisablePocketDimension();
            return;
        }

        var highestTier = int.MinValue;
        PocketDimensionUpgrade newModule = null;
        foreach (var dimension in All)
        {
            if (dimension.DimensionTier > highestTier)
            {
                highestTier = dimension.DimensionTier;
                newModule = dimension;
            }
        }

        if (newModule == null)
        {
            Plugin.Logger.LogWarning("Failed to find a valid active dimension for the upgrade!");
            return;
        }

        var existedBefore = _currentDimensionUpgrade != null;

        if (_currentDimensionUpgrade == newModule)
        {
            return;
        }

        _currentDimensionUpgrade = newModule;

        if (existedBefore)
            ChangePocketDimension();
        else
            CreatePocketDimension();
    }

    private static void DisablePocketDimension()
    {
        Plugin.Logger.LogInfo("Disabling pocket dimension");

        DisruptPocketDimensionCreation();
        CancelTeleportIfActive();
        PdaElements.PocketDimensionButton.SetElementActive(false);
    }

    private static void CreatePocketDimension()
    {
        Plugin.Logger.LogInfo("Creating a new pocket dimension");

        DisruptPocketDimensionCreation();

        _setUpCoroutine = UWE.CoroutineHost.StartCoroutine(SetUpPocketDimensionsCoroutine());
    }

    private static void DisruptPocketDimensionCreation()
    {
        if (_setUpCoroutine != null)
        {
            if (_bufferTimeActive)
            {
                Plugin.Logger.LogWarning("PocketDimension creation interrupted!");
                UWE.CoroutineHost.StopCoroutine(_setUpCoroutine);
                _bufferTimeActive = false;
            }
            else if (_loadingPocketDimensionPrefab)
            {
                _cancelingLoading = true;
            }
        }
    }

    private static void ChangePocketDimension()
    {
        Plugin.Logger.LogInfo("Changing the current pocket dimension");
        
        CancelTeleportIfActive();
        CreatePocketDimension();
    }

    private static void CancelTeleportIfActive()
    {
        if (_teleporting && _teleportCoroutine != null)
        {
            UWE.CoroutineHost.StopCoroutine(_teleportCoroutine);
            _teleporting = false;
        }
    }

    private static IEnumerator SetUpPocketDimensionsCoroutine()
    {
        yield return new WaitUntil(() => _cancelingLoading == false);

        if (_bufferTimeActive)
        {
            Plugin.Logger.LogWarning("The pocket dimension system is already buffering!");
            yield break;
        }

        // Initial arbitrary buffer timer
        _bufferTimeActive = true;
        yield return new WaitForSecondsRealtime(BufferDuration);
        _bufferTimeActive = false;

        var dimension = _currentDimensionUpgrade;

        if (dimension == null)
        {
            Plugin.Logger.LogWarning("Failed to find active dimension upgrade! Canceling...");
            yield break;
        }

        if (PocketDimensionSub.TryGetPocketDimension(dimension.DimensionTechType, out var existingSub))
        {
            Plugin.Logger.LogInfo("Found an existing pocket dimension");
            _activeSub = existingSub;
            PdaElements.PocketDimensionButton.SetElementActive(true);
            yield break;
        }

        _loadingPocketDimensionPrefab = true;

        var task = CraftData.GetPrefabForTechTypeAsync(_currentDimensionUpgrade.DimensionTechType);
        yield return task;

        if (_cancelingLoading)
        {
            Plugin.Logger.LogWarning("Pocket dimension creation was canceled!");
            _cancelingLoading = false;
            _loadingPocketDimensionPrefab = false;
            yield break;
        }

        _loadingPocketDimensionPrefab = false;

        var prefab = task.GetResult();
        var dimensionObject = Instantiate(prefab, dimension.WorldLocation, Quaternion.identity);
        _activeSub = dimensionObject.GetComponent<PocketDimensionSub>();

        // Completion
        PdaElements.PocketDimensionButton.SetElementActive(true);
    }

    public static void OnPlayerRequestedEntrance()
    {
        if (_activeSub == null)
        {
            Plugin.Logger.LogError("Failed to find active sub!");
            return;
        }

        if (Player.main.GetCurrentSub() == _activeSub)
        {
            TeleportPlayerOut();
            return;
        }

        if (Player.main.GetVehicle() != null)
        {
            ErrorMessage.AddMessage(Language.main.Get("ExitVehicleToEnterPocketDimension"));
        }
        
        if (!_teleporting)
        {
            _teleportCoroutine = UWE.CoroutineHost.StartCoroutine(TeleportCoroutine(0, OnTeleportIntoSub, true));
        }
    }

    private static void OnTeleportIntoSub()
    {
        var player = Player.main;

        if (player && _activeSub)
        {
            player.SetPosition(_activeSub.entrancePosition.position);
            player.SetCurrentSub(_activeSub);
        }

        var cameraControl = MainCameraControl.main;
        if (cameraControl)
        {
            cameraControl.rotationY = 0;
            cameraControl.rotationX = 180;
        }
    }

    private static void OnTeleportToLastLocation()
    {
        var player = Player.main;

        player.SetPosition(Vector3.zero);
    }

    private static IEnumerator TeleportCoroutine(float waitDuration, Action onTeleportComplete,
        bool properTeleport = false)
    {
        _teleporting = true;

        TeleportScreenFXController teleportVfx = null;
        try
        {
            teleportVfx = MainCamera.camera.GetComponent<TeleportScreenFXController>();
            teleportVfx.StartTeleport();
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError("Exception loading teleport VFX: " + e);
        }

        var player = Player.main;

        if (player != null && player.pda != null)
        {
            player.pda.Close();
        }

        var teleportSound = new GameObject("TeleportSound").AddComponent<FMOD_CustomLoopingEmitter>();
        teleportSound.SetAsset(TeleportSound);
        teleportSound.Play();

        yield return new WaitForSeconds(1f);

        try
        {
            onTeleportComplete();
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError("Exception thrown while handling teleportation: " + e);
        }

        if (properTeleport)
        {
            Player.main.onTeleportationComplete += OnTeleportationComplete;
            Player.main.WaitForTeleportation();
            _teleportationComplete = false;
            var timeOut = Time.realtimeSinceStartup + 10f;
            while (Time.realtimeSinceStartup < timeOut && !_teleportationComplete)
            {
                yield return null;
            }
            Player.main.onTeleportationComplete -= OnTeleportationComplete;
        }
        else
        {
            yield return new WaitForSeconds(waitDuration);
        }

        yield return new WaitForSeconds(0.5f);

        if (teleportVfx != null) teleportVfx.StopTeleport();

        if (teleportSound)
        {
            teleportSound.Stop();
            Destroy(teleportSound.gameObject, 2);
        }

        _teleporting = false;
    }

    private static void OnTeleportationComplete()
    {
        _teleportationComplete = true;
    }

    public static void QueryKickOutPlayer(PocketDimensionSub playerSub)
    {
        foreach (var active in All)
        {
            if (active.DimensionTechType == playerSub.dimensionTechType)
                return;
        }

        TeleportPlayerOut();
    }

    public static void TeleportPlayerOut()
    {
        if (!_teleporting)
        {
            _teleportCoroutine = UWE.CoroutineHost.StartCoroutine(TeleportCoroutine(5, OnTeleportToLastLocation, true));
        }
    }

    public static bool GetPlayerInsideAnyPocketDimension()
    {
        return Player.main && Player.main.GetCurrentSub() == _activeSub && _activeSub != null;
    }
}