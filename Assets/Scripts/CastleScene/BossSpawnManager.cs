using System.Collections;
using UnityEngine;

public class BossSpawnManager : MonoBehaviour
{
    [Header("보스 설정")]
    public GameObject miniBoss;
    public DialogueData bossDialogueData;

    [Header("카메라 설정")]
    public Camera mainCamera;

    private Vector3 originalCameraPosition;
    private bool bossSpawned = false;
    private bool dialogueFinished = false;
    private SimpleCameraFollow cameraFollow;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        originalCameraPosition = mainCamera.transform.position;
        cameraFollow = mainCamera.GetComponent<SimpleCameraFollow>();

        if (miniBoss != null)
            miniBoss.SetActive(false);
    }

    void Update()
    {
        if (bossSpawned) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            SpawnMiniBoss();
        }
    }

    void SpawnMiniBoss()
    {
        bossSpawned = true;
        StartCoroutine(BossSpawnSequence());
    }

    IEnumerator BossSpawnSequence()
    {
        Debug.Log("보스 등장 시퀀스 시작");

        if (cameraFollow != null)
            cameraFollow.enabled = false;

        if (FadeManager.Instance != null)
            yield return StartCoroutine(FadeManager.Instance.FadeOut(0.8f));

        yield return new WaitForSeconds(0.5f);

        if (miniBoss != null)
        {
            miniBoss.SetActive(true);
        }

        // 카메라 이동 - Y축은 그대로, X축만 보스로!
        if (miniBoss != null && mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(
                miniBoss.transform.position.x,      // X: 보스 위치
                originalCameraPosition.y,           // Y: 원래 카메라 높이 유지!
                mainCamera.transform.position.z     // Z: 그대로
            );
        }

        yield return new WaitForSeconds(0.3f);

        // 4. 페이드 인
        if (FadeManager.Instance != null)
            yield return StartCoroutine(FadeManager.Instance.FadeIn(0.8f));

        yield return new WaitForSeconds(0.5f);

        // 5. 보스 대사
        if (DialogueManager.Instance != null && bossDialogueData != null)
        {
            dialogueFinished = false;
            DialogueManager.Instance.OnDialogueEnd += OnBossDialogueEnd;
            DialogueManager.Instance.StartDialogue(bossDialogueData);

            yield return new WaitUntil(() => dialogueFinished);
            DialogueManager.Instance.OnDialogueEnd -= OnBossDialogueEnd;
        }

        yield return new WaitForSeconds(0.5f);

        // 6. 페이드 아웃
        if (FadeManager.Instance != null)
            yield return StartCoroutine(FadeManager.Instance.FadeOut(0.8f));

        // 7. 카메라 복귀
        mainCamera.transform.position = originalCameraPosition;

        // 카메라 추적 재개
        if (cameraFollow != null)
            cameraFollow.enabled = true;

        yield return new WaitForSeconds(0.3f);

        // 8. 페이드 인
        if (FadeManager.Instance != null)
            yield return StartCoroutine(FadeManager.Instance.FadeIn(0.8f));

        Debug.Log("=== 보스전 시작! ===");
    }

    void OnBossDialogueEnd()
    {
        dialogueFinished = true;
    }
}