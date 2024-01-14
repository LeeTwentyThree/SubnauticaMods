using UnityEngine;

namespace TheRedPlague.Mono;

public class PlagueHeartBehavior : MonoBehaviour
{
    public static PlagueHeartBehavior main;

    private void Awake()
    {
        main = this;
    }
}