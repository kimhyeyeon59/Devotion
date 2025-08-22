using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public DialogueData dialogueData;

    public TextMeshProUGUI nameText;      // "Name_text"
    public TextMeshProUGUI bodyText;      // "Text (TMP)"
    public GameObject arrow;              // "Next_Icon"

    // 최상위 대화 UI 오브젝트들
    public GameObject dialogueBoxObject;  // "Text box"
    public GameObject nameBoxObject;      // "Text box_name"

    // 초상화 관리
    public GameObject charImageObject;    // Cha_Image 오브젝트 (부모)
    public UnityEngine.UI.Image portraitImage; // Cha_Image 안의 Image

    public TypewriterEffect typewriter;

    private int currentIndex = 0;
    private bool isWaitingForInput = false;
    private DialogueLine[] lines;
    private bool dialogueEnded = false;

    public static DialogueManager Instance { get; private set; }

    public bool IsTownNPC2TalkCompleted = false;
    public Action OnDialogueEnd;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // UI 초기화
        if (dialogueBoxObject != null) dialogueBoxObject.SetActive(false);
        if (nameBoxObject != null) nameBoxObject.SetActive(false);
        if (arrow != null) arrow.SetActive(false);
        if (charImageObject != null) charImageObject.SetActive(false); // Cha_Image 숨기기
    }

    void Start()
    {
        if (dialogueData != null)
            StartCoroutine(StartDialogueWithDelay(2f));
    }

    public void StartDialogue(DialogueData data)
    {
        dialogueEnded = false;
        dialogueData = data;

        if (dialogueBoxObject != null) dialogueBoxObject.SetActive(true);
        if (nameBoxObject != null) nameBoxObject.SetActive(false);
        if (charImageObject != null) charImageObject.SetActive(true);

        LoadDialogue(data);

        // 플레이어 이동 제한
        var player = FindObjectOfType<MyPlayerController>();
        if (player != null)
        {
            player.canMove = false;
            if (player.footstepAudio != null && player.footstepAudio.isPlaying)
                player.footstepAudio.Stop();
        }
    }

    private IEnumerator StartDialogueWithDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        if (dialogueBoxObject != null) dialogueBoxObject.SetActive(true);
        if (nameBoxObject != null) nameBoxObject.SetActive(false);
        if (charImageObject != null) charImageObject.SetActive(true);

        var player = FindObjectOfType<MyPlayerController>();
        if (player != null)
        {
            player.canMove = false;
            if (player.footstepAudio != null && player.footstepAudio.isPlaying)
                player.footstepAudio.Stop();
        }

        LoadDialogue(dialogueData);
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isWaitingForInput)
        {
            if (typewriter.IsTyping)
            {
                typewriter.Skip();
                bodyText.text = lines[currentIndex].content;
                arrow.SetActive(true);
                isWaitingForInput = true;
            }
            else
            {
                currentIndex++;
                if (currentIndex < lines.Length)
                    ShowLine();
                else
                    EndDialogue();
            }
        }
    }

    public void LoadDialogue(DialogueData data)
    {
        dialogueData = data;
        lines = dialogueData.lines;
        currentIndex = 0;
        ShowLine();
    }

    void ShowLine()
    {
        isWaitingForInput = false;
        arrow.SetActive(false);

        var line = lines[currentIndex];

        // 이름 처리
        if (string.IsNullOrEmpty(line.speaker))
        {
            if (nameBoxObject != null) nameBoxObject.SetActive(false);
        }
        else
        {
            if (nameBoxObject != null) nameBoxObject.SetActive(true);
            nameText.text = line.speaker;
        }

        // 초상화 처리
        if (portraitImage != null && charImageObject != null)
        {
            if (line.portrait != null)
            {
                portraitImage.sprite = line.portrait;
                charImageObject.SetActive(true);
                portraitImage.gameObject.SetActive(true);
            }
            else
            {
                portraitImage.gameObject.SetActive(false);
                // 초상화 없으면 부모도 숨길지 선택 가능
                // charImageObject.SetActive(false);
            }
        }

        // 텍스트 타이핑
        typewriter.StartTyping(line.content, () =>
        {
            isWaitingForInput = true;
            if (arrow != null) arrow.SetActive(true);
        });
    }

    void EndDialogue()
    {
        if (dialogueEnded) return;
        dialogueEnded = true;

        if (dialogueData != null && dialogueData.dialogueName == "Town_NPC2")
        {
            IsTownNPC2TalkCompleted = true;
            Debug.Log("Town_NPC2 대화 완료!");
        }

        if (arrow != null) arrow.SetActive(false);
        if (nameBoxObject != null) nameBoxObject.SetActive(false);
        if (dialogueBoxObject != null) dialogueBoxObject.SetActive(false);
        bodyText.text = "";
        if (charImageObject != null) charImageObject.SetActive(false);

        var player = FindObjectOfType<MyPlayerController>();
        if (player != null)
        {
            player.canMove = true;
            player.suppressJumpThisFrame = true;
        }

        TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
        if (tutorialManager != null && !tutorialManager.IsCompleted())
        {
            tutorialManager.StartTutorial();
        }

        OnDialogueEnd?.Invoke();
    }
}
