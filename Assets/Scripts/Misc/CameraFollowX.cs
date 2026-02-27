using UnityEngine;

public class CameraFollowX : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;

    private float fixedY;
    private float fixedZ;

    void Start()
    {
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (player == null) return;

        float targetX = player.position.x;

        Vector3 targetPosition = new Vector3(targetX, fixedY, fixedZ);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}