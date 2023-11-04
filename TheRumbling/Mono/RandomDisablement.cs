using UnityEngine;

namespace TheRumbling.Mono;

public class RandomDisablement : MonoBehaviour
{
    public float appearProbability;

    private void Start()
    {
        gameObject.SetActive(appearProbability >= Random.value);
    }
}