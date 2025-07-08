using System.Collections;
using System.Collections.Generic;
using Nautilus.Json;
using Nautilus.Json.Attributes;
using UnityEngine;

namespace SeaVoyager.Mono;

public class PrefabPlaceholdersGroupSafe : MonoBehaviour
{
	public UniqueIdentifier prefabIdentifier;
	public PrefabPlaceholder[] prefabPlaceholders;
	
	// This MUST be done to prevent a nasty bug with modded global prefabs being deserialized late
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);
		DelayedLoad();
	}

	private void DelayedLoad()
	{
		if (AlreadySpawned()) return;
		
		Spawn();
		MarkAsAlreadySpawned();
	}

	private bool AlreadySpawned()
	{
		if (Plugin.PrefabPlaceholdersSaveData.spawnedIds == null)
		{
			return false;
		}
		return Plugin.PrefabPlaceholdersSaveData.spawnedIds.Contains(prefabIdentifier.Id);
	}

	private void MarkAsAlreadySpawned()
	{
		if (Plugin.PrefabPlaceholdersSaveData.spawnedIds == null)
		{
			Plugin.PrefabPlaceholdersSaveData.spawnedIds = new HashSet<string>();
		}
		Plugin.PrefabPlaceholdersSaveData.spawnedIds.Add(prefabIdentifier.Id);
	}

	public void Spawn()
	{
		foreach (var prefabPlaceholder in prefabPlaceholders)
		{
			prefabPlaceholder.Spawn();
		}
	}
	

	[FileName("PrefabPlaceholders")]
	public class SaveData : SaveDataCache
	{
		public HashSet<string> spawnedIds;
	}
}
