using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UWE;

namespace TheRedPlague.Mono.PlagueGarg;

public class GargCorpseBehavior : MonoBehaviour
{
    private static List<GargCorpseBehavior> _corpses = new List<GargCorpseBehavior>();
    private static readonly int BirthParam = Animator.StringToHash("birth");

    public static void BirthPlagueGargs()
    {
        foreach (var corpse in _corpses)
        {
            corpse.StartCoroutine(corpse.BirthCoroutine());
        }
    }

    private IEnumerator BirthCoroutine()
    {
        MainCameraControl.main.ShakeCamera(2.5f, 20);
            
        foreach (var animator in gameObject.GetComponentsInChildren<Animator>())
        {
            animator.SetTrigger(BirthParam);
        }

        var endTime = Time.time + 16.583f;

        var plagueGargRequest = PrefabDatabase.GetPrefabAsync("PlagueGarg");
        yield return plagueGargRequest;
        plagueGargRequest.TryGetPrefab(out var plagueGargPrefab);
        
        while (Time.time < endTime)
        {
            yield return null;
        }

        var animatedPlagueGarg = transform.Find("Gargantuan_AdultRedPlague");
        animatedPlagueGarg.gameObject.SetActive(false);
        
        var dummy = new GameObject("dummy");
        dummy.transform.parent = transform;
        dummy.transform.localPosition = new Vector3(0, -1.2f, 94.5f);

        var plagueGarg = Instantiate(plagueGargPrefab);
        plagueGarg.transform.position = dummy.transform.position;
        
        Destroy(dummy);
    }

    private void OnEnable()
    {
        _corpses.Add(this);
    }
    
    private void OnDisable()
    {
        _corpses.Remove(this);
    }
}