using UnityEngine;

namespace TheRedPlague;

public struct InfectionSettings
{
    public Color InfectedBodyColor { get; }
    public float InfectionHeight { get; }
    public Vector3 InfectionScale { get; }
    public Vector3 InfectionOffset { get; }

    public InfectionSettings(Color infectedBodyColor, float infectionHeight = -0.1f, Vector3 infectionScale = default, Vector3 infectionOffset = default)
    {
        InfectedBodyColor = infectedBodyColor;
        InfectionHeight = infectionHeight;
        InfectionScale = infectionScale == default ? Vector3.one : infectionScale;
        InfectionOffset = infectionOffset;
    }
}