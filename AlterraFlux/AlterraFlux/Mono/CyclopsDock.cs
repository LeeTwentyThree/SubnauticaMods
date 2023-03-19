namespace AlterraFlux.Mono;

internal class CyclopsDock : MonoBehaviour
{
    public Transform waterPlane;
    public CyclopsDockDoor door1;
    public CyclopsDockDoor door2;

    private float waterLevel;

    public float GetWaterDepth()
    {
        float level1 = door1.waterLevelPoint.position.y;
        float level2 = door2.waterLevelPoint.position.y;
        return Mathf.Max(level1, level2) + 0.1f;
    }

    public bool IsClosing { get { return !door1.Open && !door2.Open; } }

    private void Update()
    {
        waterLevel = GetWaterDepth();
        if (IsClosing)
        {
            ChangeWaterLevelGradually();
        }
        else
        {
            SetLevelInstantly();
        }
    }

    private void ChangeWaterLevelGradually()
    {
        var newY = Mathf.MoveTowards(waterPlane.position.y, waterLevel, Time.deltaTime);
        waterPlane.position = new Vector3(waterPlane.position.x, newY, waterPlane.position.z);
        waterPlane.gameObject.SetActive(newY < Ocean.GetOceanLevel());
    }

    private void SetLevelInstantly()
    {
        if (!IsClosing && waterPlane.position.y > waterLevel) return;
        waterPlane.position = new Vector3(waterPlane.position.x, waterLevel, waterPlane.position.z);
        waterPlane.gameObject.SetActive(waterLevel < Ocean.GetOceanLevel());
    }
}
