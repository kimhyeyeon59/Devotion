using System.Collections;
using TMPro;
using UnityEngine;


public class TutorialManager : MonoBehaviour
{
    public GameObject hintPanel;
    public Transform player;
    public float moveThreshold = 3f;

    private Vector3 startPosition;
    private bool moveDone = false;
    private bool jumpDone = false;
    private bool tripleJumpDone = false;

    private MyPlayerController playerController;

    private bool tutorialStarted = false;
    private bool tutorialCompleted = false;

    void Start()
    {
        playerController = player.GetComponent<MyPlayerController>();
        hintPanel.SetActive(false);
    }

    void Update()
    {
        if (!tutorialStarted) return;

        float moved = Mathf.Abs(player.position.x - startPosition.x);

        if (!moveDone && moved > moveThreshold)
            moveDone = true;

        if (!jumpDone && Input.GetKeyDown(KeyCode.Space))
            jumpDone = true;

        if (!tripleJumpDone && playerController != null && playerController.CurrentJumpCount >= 2)
            tripleJumpDone = true;

        if (moveDone && jumpDone && tripleJumpDone)
        {
            hintPanel.SetActive(false);
            tutorialStarted = false;
            tutorialCompleted = true;

            Debug.Log("튜토리얼 완료됨"); // 이게 반드시 떠야 함
        }

    }

    public void StartTutorial()
    {
        if (tutorialStarted || tutorialCompleted) return; // 이 조건 반드시 있어야 함!

        startPosition = player.position;
        moveDone = false;
        jumpDone = false;
        tripleJumpDone = false;

        hintPanel.SetActive(true);
        tutorialStarted = true;

        Debug.Log("튜토리얼 시작됨");
    }


    public bool IsCompleted()
    {
        return tutorialCompleted;
    }

}

