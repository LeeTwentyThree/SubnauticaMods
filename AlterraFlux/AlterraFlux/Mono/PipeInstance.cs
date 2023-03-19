namespace AlterraFlux.Mono;

internal class PipeInstance : MonoBehaviour
{
    public PipeData data;

    public static PipeInstance CreateInstance(PipeData data)
    {
        GameObject pipe = new GameObject("Pipe");
        Transform[] transforms = new Transform[data.positions.Length];
        for (int i = 0; i < data.positions.Length; i++)
        {
            Transform point = new GameObject("PipePoint" + i).transform;
            point.transform.parent = pipe.transform;
            point.transform.position = data.positions[i];
            transforms[i] = point;
        }
        PipeGenerator gen = PipeGenerator.CreateInstance(pipe, transforms, GlobalData.pipeRadius, GlobalData.pipeQuality, GlobalData.pipeBlendFactor, null);
        gen.UpdateMesh();
        var component = pipe.AddComponent<PipeInstance>();
        component.data = data;
        return component;
    }
}
