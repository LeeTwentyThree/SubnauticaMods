namespace AlterraFlux.Mono;

public static class CurveSmoother
{
    public static void SmoothInPlaceLoop(Vector3[] curve, int iterations = 1, float alpha = 0.25f)
    {
        int N = curve.Length;
        for (int iter = 0; iter < iterations; ++iter)
        {
            for (int i = 0; i < N; ++i)
            {
                int index = (i % N);
                int iPrev = (i == 0) ? N - 1 : i - 1;
                int iNext = (i + 1) % N;

                var prev = curve[iPrev];
                var next = curve[iNext];
                var c = (prev + next) * 0.5f;

                curve[index] = (1 - alpha) * curve[index] + (alpha) * c;
            }
        }
    }

    public static void SmoothInPlaceOpen(Vector3[] curve, int iterations = 1, float alpha = 0.25f)
    {
        for (int iter = 0; iter < iterations; ++iter)
        {
            for (int i = 1; i < curve.Length - 1; ++i)
            {
                var prev = curve[i - 1];
                var next = curve[i + 1];
                var c = (prev + next) * 0.5f;

                curve[i] = (1 - alpha) * curve[i] + (alpha) * c;
            }
        }
    }
}