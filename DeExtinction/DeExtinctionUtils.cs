using UnityEngine;

namespace DeExtinction;

public static class DeExtinctionUtils
{
    // Causes a creature to get tired during the night
    public static AnimationCurve StandardActivityCurve { get; } = new AnimationCurve(
        new Keyframe(0, 0.3f, 0, 0, 0.333f, 0.333f),
        new Keyframe(0.1f, 0.3f, 0, 0.035f, 0.333f, 0.535f),
        new Keyframe(0.2f, 1f, 0, 0, 0.333f, 0.333f),
        new Keyframe(0.8f, 1f, 0, 0, 0.333f, 0.333f),
        new Keyframe(0.9f, 0.3f, 0.00999f, 0.00999f, 0.152f, 0.621f),
        new Keyframe(1f, 0.3f, 0, 0, 0.333f, 0.333f)
    );
}