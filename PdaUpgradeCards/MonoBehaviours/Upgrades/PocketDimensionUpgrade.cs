using System;
using System.Collections;
using System.Collections.Generic;
using PdaUpgradeCards.Data;
using UnityEngine;

namespace PdaUpgradeCards.MonoBehaviours.Upgrades;

public abstract class PocketDimensionUpgrade : UpgradeChipBase
{
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
        CancelTeleportIfActive();
        PdaElements.PocketDimensionButton.SetElementActive(false);
    }

    private static void CreatePocketDimension()
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
        
        _setUpCoroutine = UWE.CoroutineHost.StartCoroutine(SetUpPocketDimensionsCoroutine());
    }

    private static void ChangePocketDimension()
    {
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
        yield return new WaitForSecondsRealtime(1f);
        _bufferTimeActive = false;

        var dimension = _currentDimensionUpgrade;
        
        if (dimension == null)
        {
            Plugin.Logger.LogWarning("Failed to find active dimension upgrade! Canceling...");
            yield break;
        }
        
        if (PocketDimensionSub.TryGetPocketDimension(dimension.DimensionTechType, out _))
        {
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

        if (!_teleporting)
        {
            _teleportCoroutine = UWE.CoroutineHost.StartCoroutine(TeleportCoroutine());
        }
    }

    private static IEnumerator TeleportCoroutine()
    {
        _teleporting = true;
        
        TeleportScreenFXController teleportVfx = null;
        try
        {
            teleportVfx = MainCamera.camera.GetComponent<TeleportScreenFXController>();
            teleportVfx.Start();
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError("Exception loading teleport VFX: " + e);
        }

        yield return new WaitForSeconds(2);
        
        var player = Player.main;

        if (player)
        {
            player.SetPosition(_activeSub.entrancePosition.position);
            player.SetCurrentSub(_activeSub);
        }
        
        yield return new WaitForSeconds(0.5f);

        if (teleportVfx != null) teleportVfx.StopTeleport();

        _teleporting = false;
    }
}