using DebugHelper.Systems;
using SMLHelper.V2.Commands;
using UnityEngine;

namespace DebugHelper.Commands
{
    public static class GeneralCommands
    {
        [ConsoleCommand("createreferencepoint")]
        public static void CreateReferencePoint()
        {
            new GameObject("ReferencePoint").AddComponent<ReferencePoint>().position = SNCameraRoot.main.transform.position;
        }

        private class ReferencePoint : BasicDebugIcon
        {
            public Vector3 position;
            
            public override string Label
            {
                get
                {
                    return Mathf.Round(position.x) + ", " + Mathf.Round(position.y) + ", " + Mathf.Round(position.z) + "\n" + Mathf.Round(Vector3.Distance(position, SNCameraRoot.main.transform.position)) + "m";
                }
            }

            public override Sprite Icon => DebugIconManager.Icons.Pointer;

            public override Vector3 Position => position;

            public override float Scale => 1f;

            public override Color Color => Color.green;
        }
        
        [ConsoleCommand("drawstar")]
        public static void DrawStar(float duration, float radius = 1f)
        {
            Utils.DebugDrawStar(Player.main.transform.position, radius, Color.cyan, duration);
        }

        [ConsoleCommand("lookingat")]
        public static void LookingAt(bool hitTriggers = false)
        {
            Transform scanTransform = MainCameraControl.main.transform;
            if (Physics.Raycast(scanTransform.position + scanTransform.forward, scanTransform.forward, out RaycastHit hit, float.MaxValue, -1, hitTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore))
            {
                var hitGameObject = hit.collider.gameObject;
                var parent = hitGameObject.transform.parent;
                var attachedRb = hit.collider.attachedRigidbody;
                var root = UWE.Utils.GetEntityRoot(hitGameObject);
                ErrorMessage.AddMessage($"Raycast hit collider of name '{hitGameObject.name}'");
                if (parent != null)
                {
                    ErrorMessage.AddMessage($"Collider is direct child of '{parent.name}'");
                }
                if (attachedRb != null)
                {
                    ErrorMessage.AddMessage($"Collider is attached to the Rigidbody '{attachedRb.gameObject.name}'");
                }
                if (root != null)
                {
                    ErrorMessage.AddMessage($"Entity root of this collider is '{root.name}'");
                }
            }
            else
            {
                ErrorMessage.AddMessage("Raycast failed.");
            }
        }
    }
}
