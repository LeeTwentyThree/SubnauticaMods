namespace AlterraFlux.Mono;

internal class PipePlacement : MonoBehaviour
{
    private PipeConnectionPoint startPoint;
    private PipeConnectionPoint endPoint;

    private float searchRadius = 0.6f;

    private PipeGenerator pipeGenerator;

    private GameObject pipeGhost;
    private Transform[] positions = new Transform[0];

    private bool inited;

    private bool PlacingStartPoint => startPoint == null;

    private bool PlacingEndPoint => !PlacingStartPoint && endPoint == null;

    private IEnumerator Start()
    {
        positions = new Transform[5];

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = new GameObject("PipePosition" + i).transform;
        }

        pipeGhost = new GameObject("PipeGhost");

        var ghostMaterialTask = new TaskResult<Material>();
        yield return CommonPrefabs.GetGhostMaterial(ghostMaterialTask);

        pipeGenerator = PipeGenerator.CreateInstance(pipeGhost,
            positions,
            GlobalData.pipeRadius, GlobalData.pipeQuality, GlobalData.pipeBlendFactor,
            ghostMaterialTask.Get());

        inited = true;
    }

    private void Update()
    {
        if (!inited) return;

        Vector3 hitPoint;

        if (!Physics.Raycast(new Ray(MainCamera.camera.transform.position, MainCamera.camera.transform.forward),
            out var hit, 35f, (1 << LayerID.Default) | (1 << LayerID.TerrainCollider), QueryTriggerInteraction.Ignore))
        {
            return;
        }

        hitPoint = hit.point;

        if (GameInput.GetButtonDown(GameInput.Button.RightHand))
        {
            CancelPlacement();
        }

        HandReticle.main.SetText(HandReticle.TextType.Use, "Place pipe", false);
        HandReticle.main.SetIcon(HandReticle.IconType.Interact);

        bool placing = GameInput.GetButtonDown(GameInput.Button.LeftHand);

        PipeConnectionPoint startPointCandidate = null;
        PipeConnectionPoint endPointCandidate = null;

        if (PlacingStartPoint)
        {
            startPointCandidate = PipeConnectionPoint.GetConnectionPointInRange(hitPoint, searchRadius);
            if (startPointCandidate != null) SetEndPointPositions(startPointCandidate.Position, startPointCandidate.ForwardDirection, 0, 1);
        }
        else if (PlacingEndPoint)
        {
            endPointCandidate = PipeConnectionPoint.GetConnectionPointInRange(hitPoint, searchRadius, startPoint);
            if (endPointCandidate != null)
            {
                SetEndPointPositions(endPointCandidate.Position, endPointCandidate.ForwardDirection, 4, 3);
            }
        }

        if (PlacingStartPoint && startPointCandidate == null)
        {
            SetEndPointPositions(hitPoint, hit.normal, 0, 1);
            SetEndPointPositions(hitPoint + hit.normal, -hit.normal, 4, 3);
        }
        else if (PlacingEndPoint && endPointCandidate == null)
        {
            SetEndPointPositions(hitPoint + hit.normal, (startPoint.Position - hitPoint).normalized, 4, 3);
        }

        if (placing)
        {
            if (startPoint == null) startPoint = startPointCandidate;
            if (endPoint == null) endPoint = endPointCandidate;
            if (startPoint != null && endPoint != null)
            {
                CompletePlacement();
            }
        }

        var midPoint = Vector3.Lerp(positions[0].position, positions[4].position, 0.5f);

        positions[2].position = midPoint + GetRandomCenterOffset(midPoint);

        pipeGenerator.UpdateMesh();
    }

    private Vector3 GetRandomCenterOffset(Vector3 midPoint)
    {
        return new Vector3(Mathf.PerlinNoise(midPoint.x, 10), Mathf.PerlinNoise(midPoint.y, 20), Mathf.PerlinNoise(midPoint.z, 30)).normalized * 1.5f;
    }

    private void CompletePlacement()
    {
        Destroy(gameObject);
        PipeManager.Main.CreateAndRegisterNewPipe(PipeData.GenerateNew(startPoint, endPoint, positions));
    }

    public void CancelPlacement()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            Destroy(positions[i].gameObject);
        }
        if (pipeGhost != null) Destroy(pipeGhost.gameObject);
    }

    private void SetEndPointPositions(Vector3 endPoint, Vector3 direction, int endPointIndex, int adjacentIndex)
    {
        positions[endPointIndex].position = endPoint;
        positions[adjacentIndex].position = endPoint + direction * GlobalData.pipeRigidityDistance;
    }
}
