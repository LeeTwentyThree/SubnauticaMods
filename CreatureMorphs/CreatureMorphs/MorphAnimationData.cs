namespace CreatureMorphs;

internal static class MorphAnimationData
{
    public static void Setup()
    {
        database = new Dictionary<MorphAnimationType, MorphAnimation>()
            {
                {MorphAnimationType.Prey, new MorphAnimation(0.1f, ModAudio.MorphPreySound) },
                {MorphAnimationType.Herbivore, new MorphAnimation(0.1f, ModAudio.MorphHerbivoreSound) },
                {MorphAnimationType.Shark, new MorphAnimation(3f, ModAudio.MorphSharkSound) },
                {MorphAnimationType.Leviathan, new MorphAnimation(5f, ModAudio.MorphLeviathanSound) },
                {MorphAnimationType.Garg, new MorphAnimation(10f, ModAudio.MorphGargSound) }
            };
    }
    private static Dictionary<MorphAnimationType, MorphAnimation> database;
    public static MorphAnimation GetData(MorphAnimationType type)
    {
        return database[type];
    }
}