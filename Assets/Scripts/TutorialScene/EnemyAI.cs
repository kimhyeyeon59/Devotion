using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float detectRadius = 5f;
    public LayerMask playerLayer;

    public Transform attackPoint;
    public float attackRadius = 2f;

    public float attackCooldown = 1.5f;  // 공격 후 쉬는 시간
    private float cooldownTimer = 0f;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 쿨다운 감소는 항상 함
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // 플레이어 감지 (한 번만)
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectRadius, playerLayer);
        if (player != null && cooldownTimer <= 0f)
        {
            Attack();
        }
    }

    void Attack()
    {
        // 공격 애니메이션 트리거 발동
        if (animator != null)
            animator.SetTrigger("Attack");

        // 쿨다운 초기화
        cooldownTimer = attackCooldown;
    }

    // 애니메이션 이벤트에서 호출 — 공격 모션 끝날 때 실행
    public void AttackHit()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);
        if (hit != null)
        {
            Debug.Log("플레이어 공격 맞음!");
        }
    }

    // 씬 뷰에서 감지 및 공격 범위 확인용 (선택사항)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}
