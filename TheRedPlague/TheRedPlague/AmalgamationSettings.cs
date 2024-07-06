using UnityEngine;

namespace TheRedPlague;

public class AmalgamationSettings
{
    public ParasiteAttachPoint[] AttachPoints { get; }

    public AmalgamationSettings(ParasiteAttachPoint[] attachPoints, params AttachableParasite[] attachableCreatures)
    {
        AttachPoints = attachPoints;
    }
}

public struct ParasiteAttachPoint
{
    public string[] PathToAffectedBone { get; }
    public float Probability { get; }
    public Vector3 LocalEulerAngles { get; }
    public bool RemoveBodyPart { get; }
    public string[] UnaffectedChildObjects { get; }
    public AttachableParasite[] AttachableCreatures { get; }

    public ParasiteAttachPoint(string[] pathToAffectedBone, float probability, Vector3 localEulerAngles, bool removeBodyPart, string[] unaffectedChildObjects, params AttachableParasite[] attachableCreatures)
    {
        PathToAffectedBone = pathToAffectedBone;
        Probability = probability;
        LocalEulerAngles = localEulerAngles;
        RemoveBodyPart = removeBodyPart;
        UnaffectedChildObjects = unaffectedChildObjects;
        AttachableCreatures = attachableCreatures;
    }
}

public struct AttachableParasite
{
    public TechType Type { get; }
    public float Scale { get; }
    public string DecapitationPoint { get; }

    public AttachableParasite(TechType type, float scale = 1f, string decapitationPoint = default)
    {
        Type = type;
        Scale = scale;
        DecapitationPoint = decapitationPoint;
    }
}