using UnityEngine;

namespace SeaVoyager.Mono.SeaLion;

public class SeaLionWaterClipProxy : MonoBehaviour
{
    public Material clipMaterial;
    public Texture3D distanceFieldTexture;
    public Vector3 distanceFieldMin;
    public Vector3 distanceFieldMax;

    private Vector3 _distanceFieldSize;
    private WaterSurface _waterSurface;

    private void Start()
    {
        _waterSurface = WaterSurface.Get();

        Vector3 borderSizeScaled = default(Vector3);
        borderSizeScaled.x = _waterSurface.foamDistance / transform.lossyScale.x;
        borderSizeScaled.y = _waterSurface.foamDistance / transform.lossyScale.y;
        borderSizeScaled.z = _waterSurface.foamDistance / transform.lossyScale.z;

        // load distance field
        _distanceFieldSize = distanceFieldMax - distanceFieldMin;
        Vector3 extents = _distanceFieldSize * 0.5f + borderSizeScaled;
        Vector3 vector = (distanceFieldMin + distanceFieldMax) * 0.5f;
        CreateBoxMesh(vector, extents);

        MeshRenderer meshRenderer = gameObject.EnsureComponent<MeshRenderer>();
        meshRenderer.material = clipMaterial;
        clipMaterial = meshRenderer.material;
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        clipMaterial.SetTexture(ShaderPropertyID._DistanceFieldTexture, distanceFieldTexture);
        clipMaterial.SetVector(ShaderPropertyID._DistanceFieldMin, distanceFieldMin);
        clipMaterial.SetVector(ShaderPropertyID._DistanceFieldSizeRcp,
            new Vector3(1f / _distanceFieldSize.x, 1f / _distanceFieldSize.y, 1f / _distanceFieldSize.z));
        clipMaterial.SetFloat(ShaderPropertyID._DistanceFieldScale, 5f);
        if (_waterSurface != null)
        {
            clipMaterial.SetTexture(ShaderPropertyID._WaterDisplacementTexture, _waterSurface.GetDisplacementTexture());
            clipMaterial.SetFloat(ShaderPropertyID._WaterPatchLength, _waterSurface.GetPatchLength());
        }

        clipMaterial.EnableKeyword("SHAPE_DISTANCE_FIELD");
        clipMaterial.DisableKeyword("SHAPE_BOX");
    }

    private void CreateBoxMesh(Vector3 center, Vector3 extents)
    {
        MeshFilter meshFilter = gameObject.EnsureComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        Vector3 vector = center + new Vector3(0f - extents.x, 0f - extents.y, extents.z);
        Vector3 vector2 = center + new Vector3(extents.x, 0f - extents.y, extents.z);
        Vector3 vector3 = center + new Vector3(extents.x, 0f - extents.y, 0f - extents.z);
        Vector3 vector4 = center + new Vector3(0f - extents.x, 0f - extents.y, 0f - extents.z);
        Vector3 vector5 = center + new Vector3(0f - extents.x, extents.y, extents.z);
        Vector3 vector6 = center + new Vector3(extents.x, extents.y, extents.z);
        Vector3 vector7 = center + new Vector3(extents.x, extents.y, 0f - extents.z);
        Vector3 vector8 = center + new Vector3(0f - extents.x, extents.y, 0f - extents.z);
        Vector3[] vertices = new Vector3[24]
        {
            vector, vector2, vector3, vector4, vector8, vector5, vector, vector4, vector5, vector6,
            vector2, vector, vector7, vector8, vector4, vector3, vector6, vector7, vector3, vector2,
            vector8, vector7, vector6, vector5
        };
        int[] triangles = new int[36]
        {
            3, 1, 0, 3, 2, 1, 7, 5, 4, 7,
            6, 5, 11, 9, 8, 11, 10, 9, 15, 13,
            12, 15, 14, 13, 19, 17, 16, 19, 18, 17,
            23, 21, 20, 23, 22, 21
        };
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;
    }

    private void OnValidate()
    {
        UpdateMaterial();
    }
}