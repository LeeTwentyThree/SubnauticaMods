using System;
using System.Collections;
using Nautilus.Utility;
using Nautilus.Utility.ModMessages;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheRedPlague.Mono;

public class RocketSeaEmperor : MonoBehaviour
{
    public static void PlayCinematic(LaunchRocket rocket)
    {
        var cinematic = new GameObject("RocketSeaEmperorCinematic");
        cinematic.AddComponent<RocketSeaEmperor>()._rocket = rocket;
        TryPlayThunderstorm();
    }

    private bool _seaEmperorAppearing;
    private float _timeSeaEmperorAppearanceStarted;

    private Vector3 _seaEmperorStartPos = new Vector3(-60, 0, 0);
    private Vector3 _seaEmperorEndPos = new Vector3(-60, 60, 0);

    private LaunchRocket _rocket;

    private Transform emperorTransform;

    private static void TryPlayThunderstorm()
    {
        if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.lee23.epicweather")) return;
        
        ModMessageSystem.Send("com.lee23.epicweather", "SetWeather", "ThunderStorm");
        ModMessageSystem.Send("com.lee23.epicweather", "SetWeatherPaused", true);
    }
    
    private IEnumerator Start()
    {
        var request = CraftData.GetPrefabForTechTypeAsync(TechType.SeaEmperorJuvenile);
        yield return request;
        var emperor = Instantiate(request.GetResult());
        emperor.SetActive(true);
        ZombieManager.InfectSeaEmperor(emperor);
        emperor.GetComponent<Rigidbody>().isKinematic = true;
        emperor.GetComponent<Creature>().enabled = false;
        var rocketParent = _rocket.GetComponentInParent<Rocket>();
        emperorTransform = emperor.transform;
        emperorTransform.parent = rocketParent.transform;
        emperorTransform.localPosition = _seaEmperorStartPos;
        emperorTransform.localEulerAngles = new Vector3(17, 90,0);
        emperorTransform.localScale = Vector3.one * 3f;
        MainCamera.camera.GetComponent<TelepathyScreenFXController>().StartTelepathy(true);
        yield return new WaitForSeconds(20);
        _seaEmperorAppearing = true;
        _timeSeaEmperorAppearanceStarted = Time.time;
        MainCamera.camera.GetComponent<TelepathyScreenFXController>().StopTelepathy();
        Utils.PlayFMODAsset(AudioUtils.GetFmodAsset("SeaEmperorJumpscare"), Player.main.transform.position);
        yield return new WaitForSeconds(4.95f);
        _seaEmperorAppearing = false;
        FadingOverlay.PlayFX(Color.black, 0.1f, 0.1f, 0.1f);
        yield return new WaitForSeconds(1f);
        FadingOverlay.PlayFX(Color.black, 0.1f, 0.1f, 0.1f);
        yield return new WaitForSeconds(0.11f);
        emperorTransform.localPosition = new Vector3(-1, 46.500f, 0);
        emperorTransform.localEulerAngles = new Vector3(90, 90, 0);
        emperorTransform.localScale = Vector3.one * 0.4f;
        SpawnBlackSphere();
        yield return new WaitForSeconds(2f);
        FadingOverlay.PlayFX(Color.black, 0.1f, 10f, 1f);
        yield return new WaitForSeconds(2f);
        uSkyManager.main.RevertRocketLaunch();
        _rocket.cinematicController.OnPlayerCinematicModeEnd();
        EndCreditsManager.showEaster = false;
        AddressablesUtility.LoadSceneAsync("EndCreditsSceneCleaner", LoadSceneMode.Single);
    }

    private void Update()
    {
        if (_seaEmperorAppearing)
        {
            emperorTransform.localPosition =
                Vector3.MoveTowards(emperorTransform.localPosition, _seaEmperorEndPos, Time.deltaTime * 30f);
        }
    }

    private void SpawnBlackSphere()
    {
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * 7;
        sphere.transform.position = MainCamera.camera.transform.position + new Vector3(0, -0.5f, 0);
        sphere.GetComponent<Collider>().enabled = false;
        var renderer = sphere.GetComponent<MeshRenderer>();
        var material = renderer.material;
        material.shader = MaterialUtils.Shaders.MarmosetUBER;
        material.SetFloat(ShaderPropertyID._MyCullVariable, 0);
        material.color = Color.black;
        material.SetColor(ShaderPropertyID._SpecColor, Color.black);
    }
}