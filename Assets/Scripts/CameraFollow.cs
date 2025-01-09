using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D targetRb;
    public float minY = 0f;
    public float smoothSpeed = 0.1f;
    public float upwardThreshold = 0.5f;
    public float downwardFollDist = 3f;
    
    private Vector3 currVelocity;
    private float initCameraY;

    private void Start()
    {
        initCameraY = transform.position.y;
    }

    private void LateUpdate()
    {        
        if (target.position.y > transform.position.y + upwardThreshold || targetRb.linearVelocityY > 0)
        {
            Vector3 newPos = new Vector3(transform.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, newPos, ref currVelocity, smoothSpeed);

            initCameraY = transform.position.y;
        } else if (transform.position.y > target.position.y && transform.position.y > initCameraY - downwardFollDist)
        {
            Vector3 targetPos = new Vector3(transform.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currVelocity, smoothSpeed);
        }

        if(transform.position.y < minY)
        {
        transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }
    }
}
