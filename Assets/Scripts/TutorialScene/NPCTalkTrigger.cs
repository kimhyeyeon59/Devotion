using UnityEngine;

public class NPCTalkTrigger : MonoBehaviour
{
    public Transform player;  // 플레이어 Transform
    private MyPlayerController playerController;
    private GameObject currentNPC;

    private void Start()
    {
        if (player != null)
            playerController = player.GetComponent<MyPlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            currentNPC = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC") && other.gameObject == currentNPC)
        {
            currentNPC = null;
        }
    }

    private void Update()
    {
        if (currentNPC != null && (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt)))
        {
            var dialogueHolder = currentNPC.GetComponent<NPCDialogueHolder>();
            var npcMovement = currentNPC.GetComponent<NPCMovement>(); // 추가
            if (dialogueHolder != null)
            {
                var data = dialogueHolder.GetDialogueData();
                if (data != null)
                {
                    var dialogueManager = FindObjectOfType<DialogueManager>();
                    if (dialogueManager != null)
                    {
                        dialogueManager.StartDialogue(data);
                        dialogueHolder.MarkAsTalked();
                    }

                    if (npcMovement != null)
                        npcMovement.StartTalking(); // NPC 멈춤

                    if (playerController != null)
                        playerController.canMove = false;

                    var bubbleController = currentNPC.GetComponent<NPCBubbleController>();
                    if (bubbleController != null)
                    {
                        bubbleController.OnPlayerInteracted();
                    }

                    // 대화 종료 후 NPC 이동 재개 & 플레이어 이동 재개
                    // DialogueManager에서 EndDialogue 이벤트 호출 시 아래처럼 연결
                    dialogueManager.OnDialogueEnd = () =>
                    {
                        if (npcMovement != null)
                            npcMovement.StopTalking();

                        if (playerController != null)
                            playerController.canMove = true;
                    };
                }
            }
        }
    }
}
