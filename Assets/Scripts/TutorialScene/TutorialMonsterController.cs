using UnityEngine;
using System.Collections;


public class TutorialMonsterController : MonoBehaviour
{
    public float detectRadius = 6f;
    public float attackRadius = 3f;
    public float attackCooldown = 1.5f;
    public LayerMask playerLayer;
    public Transform player;

    public int maxHitPoints = 3;
    private int currentHitPoints;
    private bool isDead = false;

    private Animator animator;
    private float cooldownTimer = 0f;

    public DialogueData townNPC2DialogueData;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHitPoints = 0;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (isDead) return;

        if (!DialogueManager.Instance.IsTownNPC2TalkCompleted) return;

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (cooldownTimer <= 0f &&
            Vector2.Distance(transform.position, player.position) <= detectRadius &&
            !state.IsTag("Attacking") &&
            !state.IsTag("Hurt"))
        {
            AttackPlayer();
        }
    }


    void AttackPlayer()
    {
        animator.SetTrigger("Attack");
        cooldownTimer = attackCooldown;
    }

    // 애니메이션 이벤트에서 호출되는 공격 판정 함수
    public void AttackHit()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);
        if (hit != null)
        {
            MyPlayerController player = hit.GetComponent<MyPlayerController>();
            if (player != null && !player.isDead) // ✅ 죽은 플레이어는 무시
            {
                player.TakeDamage(1);
            }
        }
    }

    public void TakeDamage()
    {
        if (isDead) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsTag("Hurt") || state.IsTag("Death")) return;

        currentHitPoints++;
        animator.SetTrigger("Hurt");

        if (currentHitPoints >= maxHitPoints)
        {
            Die();
        }
    }


    void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");

        // 먼저 대화 시작 코루틴 실행 (2초 후)
        if (townNPC2DialogueData != null)
            StartCoroutine(StartDialogueAfterDelay(2.5f));

        // Destroy는 코루틴 안에서 실행
    }

    IEnumerator StartDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        DialogueManager.Instance.StartDialogue(townNPC2DialogueData);

        // 몬스터 삭제는 대화 시작 이후에!
        Destroy(gameObject, 2f); // 죽는 애니메이션 끝날 시간 정도
    }


    void StartTownNPC2Dialogue()
    {
        DialogueManager.Instance.StartDialogue(townNPC2DialogueData);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
