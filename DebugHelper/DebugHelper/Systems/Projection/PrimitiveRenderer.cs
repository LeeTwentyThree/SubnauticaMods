using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Math = System.Math;

namespace DebugHelper.Systems.Projection
{
    internal class PrimitiveRenderer
    {
        private const float zClipping = 0.01f;
        public List<Line> lines;

        private Camera cam;
        private Texture2D currentTexture;

        private Vector2Int lastSize;

        private int pixelCount;
        private Color32[] alphaFill;
        private bool empty;
        private int minX = 0;
        private int minY = 0;
        private int maxX = 0;
        private int maxY = 0;
        private int maxPixelsPerLine;

        public bool Rendered { get; private set; }

        public PrimitiveRenderer()
        {
            cam = MainCamera.camera;
        }

        public void RenderToTexture(Texture2D texture, bool clearLines)
        {
            if (currentTexture != texture) Rendered = false;
            currentTexture = texture;
            var size = GetTextureSize(currentTexture);
            if (size != lastSize) AccomodateNewTextureSize(size);
            ClearTexture();
            foreach (var line in lines)
            {
                DrawLine(line.color, ProjectPositionTo2D(line.positionA), ProjectPositionTo2D(line.positionB));
            }
            texture.Apply();
            Rendered = true;
            var newList = new List<Line>();
            foreach (var l in lines)
            {
                if (!l.Ended) newList.Add(l);
            }
            lines = newList;
        }

        private Vector3 ProjectPositionTo2D(Vector3 pos3D)
        {
            var screenPoint = cam.WorldToScreenPoint(pos3D);
            var scaled = screenPoint / Main.config.DebugOverlayResolutionDivisor;
            return scaled;
        }

        private static Vector2Int GetTextureSize(Texture2D givenTexture)
        {
            return new Vector2Int(givenTexture.width, givenTexture.height);
        }

        private static float Distance(float a, float b)
        {
            return Mathf.Sqrt(a * a + b * b);
        }

        private static float GetProgress(int x0, int x1, int x)
        {
            return Mathf.InverseLerp(x0, x1, x);
        }

        private static float ApproximateZ(int start, int end, int current, float z0, float z1)
        {
            return Mathf.Lerp(z0, z1, GetProgress(start, end, current));
        }

        private void AccomodateNewTextureSize(Vector2Int size)
        {
            maxX = size.x;
            maxY = size.y;
            pixelCount = size.x * size.y;
            alphaFill = new Color32[pixelCount];
            for (int i = 0; i < alphaFill.Length; i++)
            {
                alphaFill[i] = new Color32(0, 0, 0, 0);
            }
            lastSize = size;
            maxPixelsPerLine = (int)Mathf.Sqrt(maxX * maxX + maxY * maxY);
            Rendered = false;
        }

        private void ClearTexture()
        {
            if (empty)
            {
                return;
            }
            currentTexture.SetPixels32(alphaFill);
            empty = true;
        }

        private void DrawLine(Color color, Vector3 pointA, Vector3 pointB)
        {
            int x0 = (int)pointA.x, y0 = (int)pointA.y, x1 = (int)pointB.x, y1 = (int)pointB.y;
            float z0 = pointA.z, z1 = pointB.z;
            if (Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
                if (x0 > x1)
                    PlotLineLow(color, x1, y1, x0, y0, z0, z1);
                else
                    PlotLineLow(color, x0, y0, x1, y1, z0, z1);
            else
            {
                if (pointA.y > pointB.y)
                    PlotLineHigh(color, x1, y1, x0, y0, z0, z1);
                else
                    PlotLineHigh(color, x0, y0, x1, y1, z0, z1);
            }
        }

        private void PlotLineLow(Color32 color, int x0, int y0, int x1, int y1, float z0, float z1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int yi = 1;
            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }
            int D = (2 * dy) - dx;
            int y = y0;

            int pixels = 0;

            for (int x = x0; x <= x1; x++)
            {
                var zApprox = ApproximateZ(x0, x1, x, z0, z1);
                if (y >= minY && y <= maxY && x >= minX && x <= maxX && zApprox > zClipping)
                {
                    currentTexture.SetPixel(x, y, color);
                    empty = false;
                }
                pixels++;
                if (pixels > maxPixelsPerLine) return;
                if (D > 0)
                {
                    y += yi;
                    D += (2 * (dy - dx));
                }
                else
                {
                    D += 2 * dy;
                }
            }
        }

        private void PlotLineHigh(Color color, int x0, int y0, int x1, int y1, float z0, float z1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int xi = 1;
            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }
            int D = (2 * dx) - dy;
            int x = x0;

            int pixels = 0;

            for (int y = y0; y <= y1; y++)
            {
                var zApprox = ApproximateZ(y0, y1, y, z0, z1);
                if (y >= minY && y <= maxY && x >= minX && x <= maxX && zApprox > zClipping)
                {
                    currentTexture.SetPixel(x, y, color);
                    empty = false;
                }
                pixels++;
                if (pixels > maxPixelsPerLine) return;
                if (D > 0)
                {
                    x += xi;
                    D += (2 * (dx - dy));
                }
                else
                {
                    D += 2 * dx;
                }
            }
        }

        public void Release()
        {
            lines = new List<Line>();
            alphaFill = null;
        }
    }
}
