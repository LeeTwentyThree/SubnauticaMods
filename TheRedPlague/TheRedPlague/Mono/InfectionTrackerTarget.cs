using UnityEngine;

namespace TheRedPlague.Mono;

public class InfectionTrackerTarget : MonoBehaviour
{
    public static InfectionTrackerTarget main;

    private void Awake()
    {
        main = this;
    }
}