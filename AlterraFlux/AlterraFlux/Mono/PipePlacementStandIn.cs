namespace AlterraFlux.Mono;

internal class PipePlacementStandIn : MonoBehaviour
{
    public static PipePlacement currentPlacementController;

    private void OnEnable()
    {
        if (currentPlacementController != null)
        {
            currentPlacementController.CancelPlacement();
        }
        currentPlacementController = new GameObject("PipePlacement").AddComponent<PipePlacement>();
    }

    private void Update()
    {
        Builder.End();
    }
}
