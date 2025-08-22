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

        // 하트(체력) 풀 회복
        PlayerStats.Instance.Heal(PlayerStats.Instance.MaxHealth);

        if (interactionText != null)
            interactionText.SetActive(false);
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
