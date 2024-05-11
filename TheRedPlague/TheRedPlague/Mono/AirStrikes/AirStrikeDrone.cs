using System.Collections;
using TheRedPlague.Mono.FleshBlobs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheRedPlague.Mono.AirStrikes;

public class AirStrikeDrone : MonoBehaviour
{
    private float _dropDelay;
    private float _velocity;
    private float _maxDepth;
    
    private float _dropBombTime;

    private bool _droppedBombs;

    private int _bombsToDrop = 3;
    private float _delayBetweenDrops = 0.5f;
    private float _bombFleshBlobsDistance = 80;

    private bool _attackedFleshBlob;

    public void SetUp(float dropDelay, float velocity, float maxDepth)
    {
        _dropDelay = dropDelay;
        _velocity = velocity;
        _maxDepth = maxDepth;
    }
    
    private void Start()
    {
        _dropBombTime = Time.time + _dropDelay + Random.Range(-0.5f, 0.5f);
        InvokeRepeating(nameof(TargetFleshBlobs), 1, 0.5f);
    }

    private void Update()
    {
        transform.position += transform.forward * _velocity * Time.deltaTime;
        if (!_droppedBombs && Time.time > _dropBombTime)
        {
            StartCoroutine(DropBombCoroutine());
            Destroy(gameObject, 120);
            _droppedBombs = true;
        }
    }

    private void TargetFleshBlobs()
    {
        if (_attackedFleshBlob) return;
        var closestFleshBlob = FleshBlobGrowth.GetClosestForDroneStrike(transform.position, _bombFleshBlobsDistance);
        if (closestFleshBlob != null)
        {
            DropBomb();
            _attackedFleshBlob = true;
        }
    }

    private IEnumerator DropBombCoroutine()
    {
        for (int i = 0; i < _bombsToDrop; i++)
        {
            DropBomb();
            yield return new WaitForSeconds(_delayBetweenDrops);
        }
    }
    
    private void DropBomb()
    {
        var bomb = Instantiate(AirStrikeController.GetOrCreateInstance().BombModel, transform.position + Vector3.down, Quaternion.identity);
        bomb.SetActive(true);
        bomb.transform.localEulerAngles = Vector3.forward * -90;
        bomb.GetComponent<Rigidbody>().velocity = Vector3.down * 10;
        bomb.GetComponent<DetonateBombInWater>().maxDepth = _maxDepth;
    }
}