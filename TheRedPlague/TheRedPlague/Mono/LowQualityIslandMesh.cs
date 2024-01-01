using System;
using UnityEngine;

namespace TheRedPlague.Mono;

public class LowQualityIslandMesh : MonoBehaviour
{
    private GameObject islandModel;

    private void Awake()
    {
        islandModel = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        islandModel.SetActive(Vector3.SqrMagnitude(transform.position + new Vector3(-64, 100, 64) - MainCamera.camera.transform.position) > 200 * 200);
    }
}