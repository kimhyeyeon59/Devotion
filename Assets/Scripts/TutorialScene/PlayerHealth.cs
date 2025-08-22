using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 외부에서 호출하는 피격 함수
    /// </summary>
    /// <param name="damage">입는 피해량 (보통 1)</param>
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        PlayerStats.Instance.TakeDamage(damage);

        if (PlayerStats.Instance.Health > 0)
        {
            animator.SetTrigger("Hurt");
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        animator.SetTrigger("Death");  // 죽는 모션 실행

        // 이동, 공격, 점프 등 막기
        if (TryGetComponent<MyPlayerController>(out var movement))
        {
            movement.enabled = false;
        }

        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = Vector2.zero;  // 멈추게 하기
        }

        // 추가로 필요한 죽음 처리 (리스폰, 게임오버 등)도 여기에
    }
}
