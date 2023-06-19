namespace CreatureMorphs.Mono;

internal class UnderControlCreatureAction : CreatureAction
{
    public override float Evaluate(Creature creature, float time)
    {
        return float.MaxValue;
    }
}