using System;
using UnityEngine;

namespace TheRedPlague.Mono;

public class LowQualityIslandMesh : MonoBehaviour
{
    public float renderDistance = 220;
    
    private GameObject _islandModel;

    private void Awake()
    {
        _islandModel = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        _islandModel.SetActive(!LaunchRocket.launchStarted &&
                              Vector3.SqrMagnitude(
                                  transform.position + new Vector3(-64, 100, 64) - MainCamera.camera.transform.position)
                              > renderDistance * renderDistance);
    }
}