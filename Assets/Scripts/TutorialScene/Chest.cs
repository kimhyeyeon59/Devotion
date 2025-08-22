using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject pressZHint;        // Press Z 문구 (비활성화된 상태로 연결)
    public GameObject potionPrefab;      // 물약 프리팹 (Project 창에서 드래그)
    public Transform player;             // 플레이어 오브젝트 (Hierarchy에서 드래그)
    public AudioClip chestOpenClip;      // 상자 여는 소리
    public AudioClip potionGetClip;      // 물약 얻는 소리

    private bool isPlayerNearby = false;
    private bool isOpened = false;
    private AudioSource audioSource;
    private UIManager uiManager;
    private MyPlayerController playerController;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        uiManager = FindObjectOfType<UIManager>();
        playerController = player.GetComponent<MyPlayerController>();

        if (pressZHint != null)
            pressZHint.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            isPlayerNearby = true;
            if (pressZHint != null)
                pressZHint.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (pressZHint != null)
                pressZHint.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNearby && !isOpened && Input.GetKeyDown(KeyCode.Z))
        {
            isOpened = true;

            if (pressZHint != null)
                pressZHint.SetActive(false);

            GetComponent<Animator>().SetBool("isOpen", true);

            if (audioSource != null && chestOpenClip != null)
            {
                Debug.Log("재생 시도: 상자 효과음");
                audioSource.volume = 0.1f;
                audioSource.PlayOneShot(chestOpenClip);
            }

            StartCoroutine(PotionSequence());
        }

        // 조작 문구는 플레이어 머리 위에 고정
        if (pressZHint != null && player != null && isPlayerNearby && !isOpened)
        {
            pressZHint.transform.position = player.position + new Vector3(-0.6f, 4.3f, 0f);
        }
    }

    IEnumerator PotionSequence()
    {
        // 움직임 제한
        if (playerController != null)
            playerController.canMove = false;

        // 물약 생성 (상자 위에서 약간 위쪽)
        Vector3 spawnPosition = transform.position + new Vector3(0f, 1.5f, 0f);
        GameObject potion = Instantiate(potionPrefab, spawnPosition, Quaternion.identity);
        potion.SetActive(true);

        // 2초 기다림
        yield return new WaitForSeconds(2f);

        Destroy(potion);

        // 물약 획득 UI 증가 + 소리 재생
        if (uiManager != null)
            uiManager.AddPotion(1);

        if (audioSource != null && potionGetClip != null)
            audioSource.PlayOneShot(potionGetClip);

        if (playerController != null)
            playerController.ShowPotionHintOnce();

        // 움직임 해제
        if (playerController != null)
            playerController.canMove = true;
    }
}
