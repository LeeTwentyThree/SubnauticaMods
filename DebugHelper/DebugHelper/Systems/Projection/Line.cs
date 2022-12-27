using UnityEngine;

namespace DebugHelper.Systems.Projection
{
    internal class Line
    {
        public Color color;
        public Vector3 positionA;
        public Vector3 positionB;
        public float duration;
        public float startTime;
        public int startFrame;

        public Line(Color color, Vector3 positionA, Vector3 positionB, float duration, float startTime, int startFrame)
        {
            this.color = color;
            this.positionA = positionA;
            this.positionB = positionB;
            this.duration = duration;
            this.startTime = startTime;
            this.startFrame = startFrame;
        }

        public float Thickness
        {
            get
            {
                return 1f;
            }
        }

        public bool Ended
        {
            get
            {
                return Time.frameCount > startFrame && Time.time > startTime + duration;
            }
        }
    }
}
