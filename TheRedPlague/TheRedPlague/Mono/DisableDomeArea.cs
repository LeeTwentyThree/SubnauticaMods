using UnityEngine;

namespace TheRedPlague.Mono;

public class DisableDomeArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = UWE.Utils.GetEntityRoot(other.gameObject);
        if (!gameObject)
        {
            gameObject = other.gameObject;
        }
        if (gameObject.GetComponent<Player>() != null)
        {
            terminal.OnTerminalAreaEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject gameObject = UWE.Utils.GetEntityRoot(other.gameObject);
        if (!gameObject)
        {
            gameObject = other.gameObject;
        }
        if (gameObject.GetComponent<Player>() != null)
        {
            terminal.OnTerminalAreaExit();
        }
    }

    public DisableDomeTerminal terminal;
    
}