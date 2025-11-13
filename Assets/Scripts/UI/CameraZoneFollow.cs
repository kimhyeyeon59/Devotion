using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;
    public float deadZoneX = 3f;

    public float minX;
    public float maxX;

    private Vector3 desiredPosition;
    private Vector3 initialPosition;

    void OnEnable()
    {
        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        initialPosition = transform.position;
        desiredPosition = initialPosition;

        FindPlayer();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            target = player.transform;
        else
            Debug.LogWarning("Player 태그가 붙은 오브젝트를 찾을 수 없습니다!");
    }

    void LateUpdate()
    {
        if (target == null) return;

        float deltaX = target.position.x - transform.position.x;

        if (Mathf.Abs(deltaX) > deadZoneX)
        {
            float moveX = deltaX - Mathf.Sign(deltaX) * deadZoneX;
            desiredPosition.x = transform.position.x + moveX;

            if (desiredPosition.x < initialPosition.x)
                desiredPosition.x = initialPosition.x;
        }
        else
        {
            desiredPosition.x = transform.position.x;
        }

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = initialPosition.y + offset.y;
        desiredPosition.z = initialPosition.z + offset.z;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
