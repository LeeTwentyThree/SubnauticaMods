using UnityEngine;

namespace DeExtinction.Mono;

public class RandomizeHungerOnStart : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Creature>().Hunger.Value = Random.value;
    }
}