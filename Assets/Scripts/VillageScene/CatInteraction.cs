using UnityEngine;
using TMPro;

public class CatInteraction : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip petSound;
    public GameObject interactionText;

    private bool isPet = false;
    private bool playerInRange = false;

<<<<<<< HEAD
=======
    public DialogueData catMessageDialogue;

>>>>>>> 20f578124 (20251112)
    void Start()
    {
        if (interactionText != null)
            interactionText.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && !isPet && Input.GetKeyDown(KeyCode.LeftShift))
        {
            PetCat();
        }
    }

    void PetCat()
    {
        isPet = true;
        animator.SetTrigger("Pet");

        if (petSound != null && audioSource != null)
            audioSource.PlayOneShot(petSound);

<<<<<<< HEAD
        // 하트(체력) 풀 회복
=======
        // 플레이어 체력 풀 회복
>>>>>>> 20f578124 (20251112)
        PlayerStats.Instance.Heal(PlayerStats.Instance.MaxHealth);

        if (interactionText != null)
            interactionText.SetActive(false);
<<<<<<< HEAD
=======

        // 대화 시작
        DialogueManager.Instance.StartDialogue(catMessageDialogue);
>>>>>>> 20f578124 (20251112)
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!isPet && interactionText != null)
                interactionText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionText != null)
                interactionText.SetActive(false);
        }
    }
}
