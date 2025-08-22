using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;
    public float deadZoneX = 3f;

    // 맵 경계 (카메라 이동 가능한 최소/최대 X 위치)
    public float minX;
    public float maxX;

    private Vector3 desiredPosition;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
        desiredPosition = initialPosition;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float deltaX = target.position.x - transform.position.x;

        if (Mathf.Abs(deltaX) > deadZoneX)
        {
            float moveX = deltaX - Mathf.Sign(deltaX) * deadZoneX;
            desiredPosition.x = transform.position.x + moveX;

            // 왼쪽 끝 고정
            if (desiredPosition.x < initialPosition.x)
                desiredPosition.x = initialPosition.x;
        }
        else
        {
            desiredPosition.x = transform.position.x;
        }

        // 카메라 X 위치 맵 범위 내로 제한
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);

        // Y, Z 위치는 offset 기준으로 고정
        desiredPosition.y = initialPosition.y + offset.y;
        desiredPosition.z = initialPosition.z + offset.z;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
