using UnityEngine;

namespace TheRedPlague.Mono.Tools;

public class InfectionTrackerTool : PlayerTool
{
    public override string animToolName => "flashlight";

    public GameObject arrowPrefab;
    public Transform arrowRoot;
    public GameObject viewModel;
    private GameObject _arrowModel;
    
    private Transform _arrow;

    private void LateUpdate()
    {
        if (Player.main == null)
            return;
        var plagueHeart = PlagueHeartBehavior.main;
        if (plagueHeart == null || !viewModel.activeSelf)
        {
            _arrowModel.SetActive(false);
            return;
        }
        _arrowModel.SetActive(true);
        var camTrans = MainCamera.camera.transform;
        _arrow.transform.position = camTrans.position + camTrans.forward * 1.3f + camTrans.up * -0.05f;
        _arrow.LookAt(plagueHeart.transform);
    }

    private void OnEnable()
    {
        _arrow = Instantiate(arrowPrefab).transform;
        _arrow.gameObject.SetActive(true);
        _arrow.transform.localScale = Vector3.one * 0.5f;
        _arrowModel = _arrow.GetChild(0).gameObject;
    }

    private void OnDisable()
    {
        if (_arrow)
        {
            Destroy(_arrow.gameObject);
        }
    }
}