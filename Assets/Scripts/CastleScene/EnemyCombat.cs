using System.Collections;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("전투 설정")]
    public float detectionRange = 3f;
    public float attackCooldown = 1.5f;
    public int maxHealth = 3;
    public int attackDamage = 1;

    private Animator animator;
    private EnemyPatrol patrolScript;
    private Transform player;
    private int currentHealth;
    private float lastAttackTime;
    private bool isDead = false;
    private bool isInCombat = false;
    private bool isAttacking = false;
    private float lostPlayerTime = 0f;

    // 다른 스크립트에서 상태 확인용
    public bool IsDead => isDead;
    public bool IsInCombat => isInCombat;

    void Start()
    {
        animator = GetComponent<Animator>();
        patrolScript = GetComponent<EnemyPatrol>();
        currentHealth = maxHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool playerInRange = distanceToPlayer <= detectionRange;

        if (playerInRange)
        {
            lostPlayerTime = 0f;

            if (!isInCombat)
            {
                isInCombat = true;
                if (patrolScript != null)
                    patrolScript.enabled = false;

                if (animator != null)
                    animator.SetBool("isWalking", false);
            }

            if (Time.time >= lastAttackTime + attackCooldown && !isAttacking)
            {
                FacePlayer();
                Attack();
            }
        }
        else
        {
            if (isInCombat)
            {
                if (lostPlayerTime == 0f)
                {
                    lostPlayerTime = Time.time;

                    if (animator != null)
                        animator.SetBool("isWalking", false);
                }

                if (Time.time >= lostPlayerTime + 2f)
                {
                    ReturnToPatrol();
                }
            }
        }
    }

    void FacePlayer()
    {
        if (player == null || isAttacking) return;

        float directionToPlayer = player.position.x - transform.position.x;
        float currentDirection = transform.localScale.x;

        if ((directionToPlayer > 0 && currentDirection < 0) ||
            (directionToPlayer < 0 && currentDirection > 0))
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        if (animator != null)
            animator.SetTrigger("Attack");
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
    }

    public void DealDamageToPlayer()
    {
        if (player != null && !isDead)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRange)
            {
                MyPlayerController playerController = player.GetComponent<MyPlayerController>();
                if (playerController != null && !playerController.isDead)
                {
                    if (PlayerStats.Instance != null)
                    {
                        playerController.TakeDamage(attackDamage);
                    }
                }
            }
        }
    }

    void ReturnToPatrol()
    {
        isInCombat = false;
        isAttacking = false;
        lostPlayerTime = 0f;

        if (patrolScript != null && !isDead)
            patrolScript.enabled = true;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        // 피격 시 잠시 멈춤
        bool wasPatrolling = false;
        if (patrolScript != null && patrolScript.enabled)
        {
            wasPatrolling = true;
            patrolScript.enabled = false;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null)
                animator.SetTrigger("Hurt");

            // Hurt 후 복귀
            if (wasPatrolling)
                StartCoroutine(ResumeAfterHurt());
        }
    }

    IEnumerator ResumeAfterHurt()
    {
        yield return new WaitForSeconds(0.5f);

        if (!isInCombat && !isDead && patrolScript != null)
            patrolScript.enabled = true;
    }

    void Die()
    {
        isDead = true;

        if (patrolScript != null)
            patrolScript.enabled = false;

        this.enabled = false;

        if (animator != null)
            animator.SetTrigger("Death");

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        Destroy(gameObject, 1f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}