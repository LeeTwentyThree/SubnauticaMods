using UnityEngine;
using DebugHelper.Commands;

namespace DebugHelper.Systems
{
    internal class ShowCollidersOnPress : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    ColliderCommands.HideColliders();
                }
                else
                {
                    ColliderCommands.ShowCollidersInRange(15f);
                }
            }
        }
    }
}
