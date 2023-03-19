namespace AlterraFlux.Mono;

public class PipeGenerator : MonoBehaviour
{
    public MeshRenderer meshRenderer { get; private set; }
    private MeshFilter meshFilter;
    private Mesh mesh;

    private Transform[] transforms;
    public float width = 2;
    public int quality = 15;
    public float alpha = 0.3f;

    public static PipeGenerator CreateInstance(GameObject go, Transform[] transforms, float width, int quality, float alpha, Material material)
    {
        var pipe = go.AddComponent<PipeGenerator>();
        pipe.meshRenderer = go.AddComponent<MeshRenderer>();
        pipe.meshRenderer.material = material;
        pipe.meshFilter = go.AddComponent<MeshFilter>();
        pipe.transforms = transforms;
        pipe.width = width;
        pipe.quality = quality;
        pipe.alpha = alpha;
        return pipe;
    }

    private void Start()
    {
        if (meshFilter == null)
        {
            Debug.LogError("Please create instances of the PipeGenerator component with PipeGenerator.CreateInstance!");
            return;
        }
        mesh = new Mesh();
        meshFilter.mesh = mesh;
        UpdateMesh();
    }

    public void UpdateTransforms(Transform[] transforms, bool updateMesh)
    {
        this.transforms = transforms;
        if (updateMesh) UpdateMesh();
    }

    public void FinalizeAndMakeStatic()
    {
        transforms = null;
    }

    public void UpdateMesh()
    {
        if (transforms == null) return;

        if (transforms.Length < 2)
        {
            SetVisible(false);
            return;
        }

        var positions = new Vector3[transforms.Length * quality];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = transforms[i / quality].position;
        }
        CurveSmoother.SmoothInPlaceOpen(positions, quality, alpha);
        CylinderGenerator.CreateMesh(ref mesh, transform.position, positions, 10, width);
        SetVisible(true);
    }

    private void SetVisible(bool visible)
    {
        meshRenderer.enabled = visible;
    }
}