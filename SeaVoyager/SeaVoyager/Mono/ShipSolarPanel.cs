using UnityEngine;

namespace SeaVoyager.Mono
{
    public class ShipSolarPanel : MonoBehaviour, IProtoEventListener
	{
		public PowerSource powerSource;
		public PowerRelay relay;
		
		private float GetSunScalar()
		{
			return DayNightCycle.main.GetLocalLightScalar();
		}

		private void Update()
		{
			powerSource.power = Mathf.Clamp(powerSource.power + (GetSunScalar() * DayNightCycle.main.deltaTime * 9), 0f, powerSource.maxPower);
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
