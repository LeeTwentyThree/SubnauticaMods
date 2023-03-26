namespace CreatureMorphs.Abilities;
internal class Bite : MorphAbility
{
    public float damage;
    public string animationParameter;
    public FMODAsset biteSound;

    protected override void OnInputReceived()
    {
        if (string.IsNullOrEmpty(animationParameter))
        {
            morphController.creature.GetAnimator().SetTrigger(animationParameter);
        }
        if (biteSound != null)
        {
            Utils.PlayFMODAsset(biteSound);
        }
    }
}