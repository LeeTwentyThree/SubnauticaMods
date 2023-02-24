using SMLHelper.V2.Json.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SeaVoyager.Ship
{
    public class ShipSolarPanel : MonoBehaviour, IProtoEventListener
	{
		public PowerSource powerSource;
		public GameObject solarPanelPrefab;
		PowerRelay relay;

		public void Initialize()
		{
			powerSource = gameObject.AddComponent<PowerSource>();
			powerSource.maxPower = Plugin.ShipMaxPower;

			relay = gameObject.AddComponent<PowerRelay>();
			relay.maxOutboundDistance = 20;
			relay.internalPowerSource = powerSource;

			powerSource.connectedRelay = relay;


			PowerFX powerFXComponent = gameObject.AddComponent<PowerFX>();
			PowerRelay referenceRelay = solarPanelPrefab.GetComponent<PowerRelay>();
			powerFXComponent.vfxPrefab = referenceRelay.powerFX.vfxPrefab;
			relay.powerFX = powerFXComponent;

			powerFXComponent.attachPoint = gameObject.transform;

			relay.outboundRelay = GetComponentInParent<PowerRelay>();
			relay.dontConnectToRelays = true;
		}
		private float GetSunScalar()
		{
			return DayNightCycle.main.GetLocalLightScalar();
		}

		private void Update()
		{
			powerSource.power = Mathf.Clamp(powerSource.power + (GetSunScalar() * DayNightCycle.main.deltaTime * Plugin.ShipMaxPowerGenerationRate), 0f, powerSource.maxPower);
		}

		public void OnProtoSerialize(ProtobufSerializer serializer)
		{
			
		}

		public void OnProtoDeserialize(ProtobufSerializer serializer)
		{
			
		}
	}
	public struct SaveData
	{
		public float power;

		public SaveData(float power)
		{
			this.power = power;
		}
	}
}
