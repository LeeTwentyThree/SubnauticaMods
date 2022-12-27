using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DebugHelper.Systems
{
    public class RenderedDamageInfo : BasicDebugIcon
    {
        public LiveMixin liveMixin;

        public float lastTime;
        public float lastDamage;
        public DamageType lastDamageType;
        public Vector3 lastDamagePosition;

        private static float maxLifetime = 10f;
        private static float maxUpwardDrift = 4f;

        private static Color undefinedColor = new Color(0f, 1f, 162f / 255f);
        private static Color explosionColor = new Color(1f, 77f / 255f, 0);
        private static Color fireColor = new Color(1f, 166f / 255f, 0);
        private static Color heatColor = new Color(1f, 0f, 0f);
        private static Color coldColor = new Color(74f / 255f, 1f, 1f);
        private static Color electricColor = new Color(169f / 255f, 166f / 255f, 1f);
        private static Color drillColor = new Color(91f / 255f, 129f / 255f, 135f / 255f);
        private static Color acidColor = new Color(97f / 255f, 207f / 255f, 19 / 255f);
        private static Color poisonColor = new Color(162f / 255f, 0f, 232f / 255f);
        private static Color punctureColor = new Color(1f, 0, 157f / 255f);
        private static Color laserCutterColor = new Color(235f / 255f, 0, 74f / 255f);
        private static Color collideColor = new Color(0f, 114f / 255f, 235f / 255f);
        private static Color radiationColor = new Color(85f / 255f, 1f, 0f);
        private static Color starveColor = new Color(158f / 255f, 90f / 255f, 0f);
        private static Color pressureColor = new Color(12f / 255f, 33 / 255f, 92 / 255f);

        public static void Create(LiveMixin liveMixin, float damage, DamageType damageType, Vector3 position)
        {
            if (Main.config.HideSmallDamageNumbers && damage < 0.1f) return;
            var go = new GameObject("DamageInfo");
            var rendered = go.AddComponent<RenderedDamageInfo>();
            rendered.liveMixin = liveMixin;
            if (position == default) rendered.lastDamagePosition = liveMixin.transform.position;
            else rendered.lastDamagePosition = position;
            rendered.lastDamagePosition += UnityEngine.Random.onUnitSphere;
            rendered.lastDamage = damage;
            rendered.lastDamageType = damageType;
            rendered.lastTime = Time.time;
        }

        public float TimePassed
        {
            get
            {
                return Time.time - lastTime;
            }
        }

        public override string Label
        {
            get
            {
                return lastDamage.ToString("F1") + " dmg\n" + lastDamageType.ToString() + "\n" + TimePassed.ToString("F1") + "s ago";
            }
        }

        private Color ColorForDamageType
        {
            get
            {
                switch (lastDamageType)
                {
                    default:
                        return undefinedColor;
                    case DamageType.Normal:
                        return Color.white;
                    case DamageType.Explosive:
                        return explosionColor;
                    case DamageType.Cold:
                        return coldColor;
                    case DamageType.Fire:
                        return fireColor;
                    case DamageType.Electrical:
                        return electricColor;
                    case DamageType.Smoke:
                        return Color.black;
                    case DamageType.Acid:
                        return acidColor;
                    case DamageType.Drill:
                        return drillColor;
                    case DamageType.Heat:
                        return heatColor;
                    case DamageType.Poison:
                        return poisonColor;
                    case DamageType.Puncture:
                        return punctureColor;
                    case DamageType.Undefined:
                        return Color.gray;
                    case DamageType.LaserCutter:
                        return laserCutterColor;
                    case DamageType.Collide:
                        return collideColor;
                    case DamageType.Radiation:
                        return radiationColor;
                    case DamageType.Starve:
                        return starveColor;
                    case DamageType.Pressure:
                        return pressureColor;
                }
            }
        }

        public override Sprite Icon => null;

        public override Vector3 Position => lastDamagePosition + Vector3.up * maxUpwardDrift * (TimePassed / maxLifetime);

        public override float Scale => 2f;

        public override Color Color => ColorForDamageType.WithAlpha(1f - TimePassed / maxLifetime);

        private void Update()
        {
            if (TimePassed >= maxLifetime || liveMixin == null)
            {
                Destroy(gameObject);
            }
        }
    }
}
