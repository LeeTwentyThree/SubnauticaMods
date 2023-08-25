using System.Collections;
using UnityEngine;
using UWE;

namespace MonkeySayMonkeyGet.VoiceCommands;

public abstract class SpawnAroundPlayerCommand : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Low;

    protected override void Perform(SpeechInput input)
    {
        CoroutineHost.StartCoroutine(SpawnAsync());
    }

    private IEnumerator SpawnAsync()
    {
        var request = PrefabDatabase.GetPrefabAsync(ClassId);
        yield return request;
        request.TryGetPrefab(out var prefab);
        var amt = Random.Range(MinSpawns, MaxSpawns);
        var origin = MainCamera.camera.transform.position;
        for (int i = 0; i < amt; i++)
        {
            Object.Instantiate(prefab, origin + (Random.insideUnitSphere * SpawnRadius), Random.rotation);
        }
    }

    public abstract string ClassId { get; }

    public abstract int MinSpawns { get; }

    public abstract int MaxSpawns { get; }

    public abstract float SpawnRadius { get; }

    public override float MinimumDelay => 3f;
}