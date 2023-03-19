namespace AlterraFlux.Mono;

internal class PipeConnectionPoint : MonoBehaviour
{
    public int relativeId;

    private static List<PipeConnectionPoint> connectionPoints = new List<PipeConnectionPoint>();

    public Vector3 Position => transform.position;
    public Vector3 ForwardDirection => transform.forward;

    private PrefabIdentifier parentIdentifier;

    public string UUID
    {
        get
        {
            if (parentIdentifier == null) parentIdentifier = GetComponentInParent<PrefabIdentifier>();
            return parentIdentifier.Id;
        }
    }

    private void OnEnable()
    {
        connectionPoints.Add(this);
    }

    private void OnDisable()
    {
        connectionPoints.Remove(this);
    }

    public static PipeConnectionPoint GetConnectionPointInRange(Vector3 point, float radius, PipeConnectionPoint exclude = null)
    {
        var sqrRadius = radius * radius;
        foreach (var connectionPoint in connectionPoints)
        {
            if (connectionPoint != exclude && !connectionPoint.Occupied && Vector3.SqrMagnitude(connectionPoint.transform.position - point) < sqrRadius)
            {
                return connectionPoint;
            }
        }
        return null;
    }

    public bool Occupied
    {
        get
        {
            return false;
        }
    }
}