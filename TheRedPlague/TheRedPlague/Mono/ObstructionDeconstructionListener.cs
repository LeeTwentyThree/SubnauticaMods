using UnityEngine;

namespace TheRedPlague.Mono;

public class ObstructionDeconstructionListener : MonoBehaviour
{
    public CyclopsClogEngines clog;
    
    public void OnKill()
    {
        clog.OnKill();
    }
}