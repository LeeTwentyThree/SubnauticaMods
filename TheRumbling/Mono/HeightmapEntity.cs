using UnityEngine;
using UnityEngine.Serialization;
using WorldHeightLib;

namespace TheRumbling.Mono;

public class HeightmapEntity : MonoBehaviour
{
    public float walkVelocity;
    public float walkingGravity = 9.8f;
    public float swimmingGravity = -9.8f;
    public float minYLevelForWalking;
    public float stickToGroundOffset;
    
    public bool Swimming { get; private set; }
    public float CurrentHeightmapHeight { get; private set; }
    public float CurrentStandingHeight => CurrentHeightmapHeight + stickToGroundOffset;

    private float _yVelocity = 0f;

    public bool canMove = true;
    
    // this field can vary for some variation between titans (must be public because Unity is confusing)
    public float maxYLevelForSwimming;

    public void SetEssentials(float walkVelocity, float walkingGravity, float swimmingGravity, float minYLevelForWalking, float maxYLevelForSwimmingMin, float maxYLevelForSwimmingMax, float stickToGroundOffset)
    {
        this.walkVelocity = walkVelocity;
        this.walkingGravity = walkingGravity;
        this.swimmingGravity = swimmingGravity;
        this.minYLevelForWalking = minYLevelForWalking;
        maxYLevelForSwimming = Random.Range(maxYLevelForSwimmingMin, maxYLevelForSwimmingMax);
        this.stickToGroundOffset = stickToGroundOffset;
    }

    private void Update()
    {
        UpdateHeightmapInfo();

        if (!canMove)
        {
            return;
        }
        
        var pos = transform.position;
        bool snappingToGround = false;
        
        if (Swimming)
        {
            // at surface
            if (Mathf.Abs(pos.y - (Ocean.GetOceanLevel() + maxYLevelForSwimming)) <= 3f)
            {
                _yVelocity = 0f;
            }
            // falling from above water
            else if (pos.y > Ocean.GetOceanLevel() + maxYLevelForSwimming)
            {
                _yVelocity += Time.deltaTime * walkingGravity;
            }
            // swimming normally (buoyant)
            else
            {
                _yVelocity += Time.deltaTime * swimmingGravity;
            }
        }
        // walking
        else
        {
            // on ground
            if (Mathf.Abs(pos.y - (Ocean.GetOceanLevel() + CurrentStandingHeight)) <= 3f)
            {
                _yVelocity = 0f;
            }
            // under the ground (should be forced upward)
            else if (pos.y < CurrentStandingHeight)
            {
                _yVelocity = 10;
            }
            // falling
            else
            {
                _yVelocity += Time.deltaTime * walkingGravity;
            }
        }
        
        SetYPosition(pos.y + _yVelocity * Time.deltaTime);
        
        transform.position += transform.forward * (walkVelocity * Time.deltaTime);
    }

    private void UpdateHeightmapInfo()
    {
        CurrentHeightmapHeight = HeightMap.Instance.TryGetValueAtPosition(new Vector2(transform.position.x, transform.position.z), out var heightValue) ? heightValue : float.MinValue;
        if (CurrentHeightmapHeight > 0 &&
            Vector3.Distance(MainCamera.camera.transform.position, transform.position) > 500)
        {
            CurrentHeightmapHeight = 0;
        }
        Swimming = CurrentStandingHeight < minYLevelForWalking;
    }

    private void SetYPosition(float newY)
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, newY, pos.z);
    }
}