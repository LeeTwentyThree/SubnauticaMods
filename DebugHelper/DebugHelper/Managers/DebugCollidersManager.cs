using DebugHelper.Basis;
using DebugHelper.Pools;
using DebugHelper.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UWE;

namespace DebugHelper.Managers
{
    public class DebugCollidersManager : BaseManager
    {
        public static DebugCollidersManager main;
        public ColliderPool pool { get; private set; }

        private Material m_vsMaterial;

        private Color m_GnClColor; // generic colliders
        private Color m_PhClColor; // colliders on non-kinematic rigidbodies
        private Color m_TgClColor; // colliders with isTrigger set to true
        private Color m_MhClColor; // anything with the MeshCollider component

        public bool enabledShowInRange { get; private set; }
        public float targetRange { get; private set; }

        private Transform m_camTransform 
        {
            get
            {
                Transform root = SNCameraRoot.main.transform;
                if (root != null) return root;
                return Camera.main.transform;
            }
        }

        public static void CreateInstance()
        {
            GameObject go = new GameObject("DebugCollidersManager");
            go.AddComponent<DebugCollidersManager>();
        }

        private void OnEnable()
        {
            main = this;
            enabledShowInRange = false;
            targetRange = 10f;
            pool = new ColliderPool();
            CoroutineHost.StartCoroutine(f_setupMaterials());
            StartTicking(0);
        }
        private void OnDisable()
        {
            CoroutineHost.StartCoroutine(c_hideColliders());
            StopTicking();
        }
        #region API
        public void ShowCollidersRange(float range)
        {
            this.targetRange = Mathf.Clamp(range, 0.01f, 1700f);
            enabledShowInRange = true;
            Tick();
        }
        public void HideColliders() => CoroutineHost.StartCoroutine(c_hideColliders());
        public override void Tick()
        {
            if (enabledShowInRange) CoroutineHost.StartCoroutine(c_drawCollidersRange());
        }
        #endregion
        #region coroutines
        private IEnumerator c_drawCollidersRange()
        {
            List<Collider> casted = m_getCollidersRange(m_camTransform.position, targetRange).ToList();
            var pooled = pool.ToSet();
            foreach (BaseDebugCollider p in pooled)
            {
                Collider c = p.Get();
                switch (casted.Contains(c))
                {
                    case true:
                        casted.Remove(c);
                        continue;
                    case false:
                        pool.Unregister(p);
                        continue;
                }
            }
            foreach (Collider c in casted)
            {
                if (c.GetComponentInParent<Player>() != null) continue;
                BaseDebugCollider nc = pool.Register(c);
                f_renderCollider(nc);
            }
            yield return null;
        }
        private IEnumerator c_hideColliders()
        {
            enabledShowInRange = false;
            pool.Clear();
            yield return null;
        }
        private IEnumerator f_setupMaterials()
        {
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.StasisRifle);
            yield return task;

            GameObject stasisRifle = task.GetResult();
            var stasisBall = stasisRifle.GetComponent<StasisRifle>().effectSpherePrefab.GetComponentInChildren<Renderer>();

            m_vsMaterial = new Material(stasisBall.materials[1]);
            m_vsMaterial.color = new Color(1f, 0.7f, 0.8f);

            m_GnClColor = new Color(1f, 1f, 0f);
            m_PhClColor = new Color(1f, 0.2f, 0.2f);
            m_TgClColor = new Color(0.5f, 0.5f, 0.5f);
            m_MhClColor = new Color(0.2f, 0.2f, 1f);
        }
        #endregion
        #region helpers
        private void f_renderCollider(BaseDebugCollider collider)
        {
            collider.CreateVisual();
            collider.SetMaterial(m_vsMaterial);
            collider.SetColor(f_colliderColor(collider));
        }
        private Collider[] m_getCollidersRange(Vector3 position, float range) => Physics.OverlapSphere(position, range);
        private Collider[] m_getCollidersRay(Ray ray, float range) => throw new NotImplementedException();
        private Color f_colliderColor(BaseDebugCollider collider)
        {
            if (collider.IsTrigger) return m_TgClColor;
            if (collider.Type == ColliderType.Rigidbody) return m_PhClColor;
            if (collider.Shape == ColliderShape.Mesh) return m_MhClColor;
            return m_GnClColor;
        }
        #endregion
    }
}
