using UnityEngine;

namespace MonkeySayMonkeyGet.Mono;

public class FreezePlayer : MonoBehaviour
{
    private void Start()
    {
        Player.main.rigidBody.isKinematic = true;
        Destroy(this, 5f);
    }

    private void OnDestroy()
    {
        Player.main.rigidBody.isKinematic = false;
    }
}
