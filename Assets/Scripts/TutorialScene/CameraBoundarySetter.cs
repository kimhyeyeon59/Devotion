using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraBoundarySetter : MonoBehaviour
{
    public SimpleCameraFollow cameraFollow;  // 카메라 스크립트 연결
    public Tilemap tilemap;                   // 맵 타일맵 연결

    void Start()
    {
        if (cameraFollow == null || tilemap == null)
        {
            Debug.LogError("CameraBoundarySetter: cameraFollow 또는 tilemap이 할당되지 않았습니다.");
            return;
        }

        Bounds bounds = tilemap.localBounds;

        float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;

        // minX와 maxX 계산
        cameraFollow.minX = bounds.min.x + cameraHalfWidth;
        cameraFollow.maxX = bounds.max.x - cameraHalfWidth;
    }
}
