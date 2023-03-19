using SMLHelper.Json;

namespace AlterraFlux.Mono;

internal class PipeManager : MonoBehaviour
{
    public static PipeManager Main
    {
        get
        {
            if (m_main == null)
            {
                m_main = new GameObject("PipeManager").AddComponent<PipeManager>();
                m_main.Init();
            }
            return m_main;
        }
    }

    private static PipeManager m_main;

    private static SavePipes saveData { get; } = SaveDataHandler.RegisterSaveDataCache<SavePipes>();

    private Dictionary<string, PipeInstance> pipeInstances = new Dictionary<string, PipeInstance>();

    private void Init()
    {
        saveData.Load();
        if (saveData.data == null)
        {
            saveData.data = new Dictionary<string, PipeData>();
        }
        foreach (var pipe in saveData.data.Values)
        {
            CreatePipe(pipe);
        }
    }

    public PipeInstance CreateAndRegisterNewPipe(PipeData data)
    {
        var pipe = CreatePipe(data);
        saveData.data.Add(data.id, data);
        return pipe;
    }

    private PipeInstance CreatePipe(PipeData data)
    {
        var pipe = PipeInstance.CreateInstance(data);
        pipeInstances.Add(data.id, pipe);
        return pipe;
    }
}
[System.Serializable]
internal class SavePipes : SaveDataCache
{
    public Dictionary<string, PipeData> data;
}
[System.Serializable]
internal class PipeData
{
    public string id;
    public string connectorAId;
    public string connectorBId;
    public Vector3[] positions;

    private PipeData(string id, string connectorAId, string connectorBId, Vector3[] positions)
    {
        this.id = id;
        this.connectorAId = connectorAId;
        this.connectorBId = connectorBId;
        this.positions = positions;
    }

    public static PipeData GenerateNew(PipeConnectionPoint conA, PipeConnectionPoint conB, Transform[] transforms)
    {
        Vector3[] positions = new Vector3[transforms.Length];
        for (int i = 0; i < transforms.Length; i++)
        {
            positions[i] = transforms[i].position;
        }

        return new PipeData(System.Guid.NewGuid().ToString(), conA.UUID, conB.UUID, positions);
    }
}