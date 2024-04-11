using UnityEngine;

namespace EpicImprovedTextures;

public class CatFace : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.LookAt(Player.main.transform);
    }
}