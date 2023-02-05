using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaTruckBranchModule;

internal class BranchModuleController : MonoBehaviour
{
    private PowerRelay myPowerRelay;

    public SeaTruckConnection rear;
    public SeaTruckConnection left;
    public SeaTruckConnection right;

    private void Start()
    {
        myPowerRelay = GetComponent<PowerRelay>();
    }

    private void Update()
    {
        if (left.connection != null && left.connection.truckSegment != null)
        {
            var techType = GetTechType(left.connection.truckSegment.gameObject);
            left.connection.truckSegment.updateDockedPosition = false;
            left.connection.truckSegment.transform.parent = transform;
            left.connection.truckSegment.transform.localPosition = new Vector3(techType == TechType.SeaTruckAquariumModule ? -6.2f : - 5.3f, 0, 1.66f);
            left.connection.truckSegment.transform.localEulerAngles = Vector3.up * 90;
            if (left.connection.truckSegment == left.truckSegment) left.connection.Disconnect();
        }
        if (right.connection != null && right.connection.truckSegment != null)
        {
            var techType = GetTechType(right.connection.truckSegment.gameObject);
            right.connection.truckSegment.updateDockedPosition = false;
            right.connection.truckSegment.transform.parent = transform;
            right.connection.truckSegment.transform.localPosition = new Vector3(techType == TechType.SeaTruckAquariumModule ? 6.2f : 5.34f, 0, 1.66f);
            right.connection.truckSegment.transform.localEulerAngles = Vector3.up * 270;
            if (left.connection.truckSegment == left.truckSegment) left.connection.Disconnect();
        }
        if (rear.connection != null && rear.connection.truckSegment != null)
        {
            if (rear.connection.truckSegment == rear.truckSegment) rear.connection.Disconnect();
            if (rear.connection == right.connection) rear.connection.Disconnect();
            if (rear.connection == left.connection) rear.connection.Disconnect();
        }
        foreach (var relay in gameObject.GetComponentsInChildren<PowerRelay>())
        {
            if (relay != myPowerRelay)
            {
                relay.inboundPowerSources = myPowerRelay.inboundPowerSources;
            }
        }
    }

    private TechType GetTechType(GameObject obj)
    {
        TechType techType = TechType.SeaTruckFabricatorModule;
        var prefabIdentifier = obj.GetComponent<PrefabIdentifier>();
        if (prefabIdentifier != null && prefabIdentifier.ClassId == "342a721d-8b6a-4367-a508-2165e42a9fa8") techType = TechType.SeaTruckAquariumModule;
        return techType;
    }
}
