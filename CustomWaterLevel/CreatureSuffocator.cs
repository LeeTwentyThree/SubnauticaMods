using UnityEngine;

namespace CustomWaterLevel;

internal class CreatureSuffocator : MonoBehaviour
{
    public Creature creature;

    private void Update()
    {
        if (transform.position.y > Plugin.WaterLevel + 3f)
        {
            creature.liveMixin.TakeDamage(20000f, transform.position);
        }
    }
}
