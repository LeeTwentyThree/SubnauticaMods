using UnityEngine;

namespace TheRedPlague.Mono;

public class MoveTowardsPlayerWhenOffScreen : MonoBehaviour
{
    public float maxDist = 10f;
    private float _timeCheckAgain;
    
    private void Update()
    {
        if (Time.time > _timeCheckAgain)
        {
            if (Vector3.SqrMagnitude(MainCamera.camera.transform.position - transform.position) < maxDist * maxDist && !JumpScareUtils.IsPositionOnScreen(transform.position))
            {
                transform.position = Vector3.MoveTowards(transform.position, MainCamera.camera.transform.position, Random.Range(1, 2));
            }
            _timeCheckAgain = Time.time + 1;
        }
    }
}