using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaTruckBranchModule;

internal class BranchModuleController : MonoBehaviour
{
    private PowerRelay myPowerRelay;

    public SeaTruckConnection left;
    public SeaTruckConnection right;

    private void Start()
    {
        myPowerRelay = GetComponent<PowerRelay>();
    }

    private void Update()
    {
        if (left.connection != null)
        {
            left.connection.truckSegment.updateDockedPosition = false;
            left.connection.truckSegment.transform.parent = transform;
            left.connection.truckSegment.transform.localPosition = new Vector3(-5.3f, 0, 1.66f);
            left.connection.truckSegment.transform.localEulerAngles = Vector3.up * 90;
        }
        if (right.connection != null)
        {
            right.connection.truckSegment.updateDockedPosition = false;
            right.connection.truckSegment.transform.parent = transform;
            right.connection.truckSegment.transform.localPosition = new Vector3(5.34f, 0, 1.66f);
            right.connection.truckSegment.transform.localEulerAngles = Vector3.up * 270;
        }
        foreach (var relay in gameObject.GetComponentsInChildren<PowerRelay>())
        {
            if (relay != myPowerRelay)
            {
                relay.inboundPowerSources = myPowerRelay.inboundPowerSources;
            }
        }
    }
}
