using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.FleshBlobs;

public class FleshBlobTornadoController : MonoBehaviour
{
    public GameObject vfxParent;
    public FMOD_CustomEmitter tornadoEmitter;

    private FleshBlobGrowth _growth;
    private float _timeUpdateAgain;
    private float _minSizeForTornado = 0.5f;

    private bool _tornadoEnabled;
    
    private float _timePlayTornadoSoundAgain;

    private bool _playedEnableTornadoNoise;

    private void Start()
    {
        _growth = GetComponent<FleshBlobGrowth>();
        vfxParent.SetActive(false);
        if (_growth.Size >= _minSizeForTornado)
        {
            _playedEnableTornadoNoise = true;
        }
    }

    private void Update()
    {
        if (Time.time > _timeUpdateAgain)
        {
            UpdateTornadoState();
            _timeUpdateAgain = Time.time + 2;
        }
        if (_tornadoEnabled && Time.time > _timePlayTornadoSoundAgain)
        {
            tornadoEmitter.Play();
            _timePlayTornadoSoundAgain = Time.time + 74;
        }
    }

    private void UpdateTornadoState()
    {
        var tornadoShouldBeEnabled = _growth.Size >= _minSizeForTornado;
        if (tornadoShouldBeEnabled == _tornadoEnabled) return;
        vfxParent.SetActive(tornadoShouldBeEnabled);
        if (!_playedEnableTornadoNoise && tornadoShouldBeEnabled)
        {
            if (Vector3.Distance(transform.position, MainCamera.camera.transform.position) < 400)
            {
                Utils.PlayFMODAsset(AudioUtils.GetFmodAsset("FleshBlobActivate"), transform.position);
            }
            _playedEnableTornadoNoise = true;
        }
        _tornadoEnabled = tornadoShouldBeEnabled;
    }
}