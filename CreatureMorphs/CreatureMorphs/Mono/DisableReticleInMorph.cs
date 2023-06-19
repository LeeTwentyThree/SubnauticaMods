namespace CreatureMorphs.Mono;

internal class DisableReticleInMorph : MonoBehaviour
{
    private void Update()
    {
        HandReticle.main.iconCanvas.gameObject.SetActive(!PlayerMorpher.PlayerCurrentlyMorphed);
    }
}
