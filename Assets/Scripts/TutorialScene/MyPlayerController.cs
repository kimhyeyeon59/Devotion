using UnityEngine;

public class MyPlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isGrounded;
    private int jumpCount;
    public int CurrentJumpCount => jumpCount;
    public bool canMove = true;

    public AudioSource footstepAudio;
    public AudioClip footstepClip;
    private bool isWalking = false;

    public AudioClip attackSfx;
    private AudioSource audioSource;
    public AudioSource attackAudioSource;
    public AudioClip hitSfx;
    public AudioClip deathSfx;
    public AudioSource sfxAudioSource;

    private UIManager uiManager;
    private bool hasShownPotionHint = false;
    public bool suppressJumpThisFrame = false;
    public bool isDead = false;

    [Header("Attack Settings")]
    public Transform attackPoint;       // 공격 위치 기준 (손 등)
    public float attackRange = 0.5f;    // 공격 범위 반지름
    public LayerMask enemyLayers;       // 몬스터가 있는 레이어

    private bool hasShownAttackHint = false;
    private bool attackHintDismissed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        jumpCount = 0;

        uiManager = FindObjectOfType<UIManager>();

        if (footstepAudio == null)
            footstepAudio = GetComponent<AudioSource>();

        if (sfxAudioSource == null)
            sfxAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAttack();
        UpdateAnimationParameters();
        HandleAttackHint();

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (uiManager != null)
            {
                uiManager.UsePotion();
            }
        }
    }

    void HandleMovement()
    {
        float moveInput = 0f;

        if (!canMove)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            // 움직일 수 없을 땐 소리도 꺼야 함
            if (footstepAudio.isPlaying)
                footstepAudio.Stop();

            isWalking = false;
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
            moveInput = -1f;
        else if (Input.GetKey(KeyCode.RightArrow))
            moveInput = 1f;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1f, 1f);

        bool wasWalking = isWalking;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isWalking = (moveInput != 0f && isGrounded);

        if (isWalking && !footstepAudio.isPlaying)
        {
            footstepAudio.clip = footstepClip;
            footstepAudio.loop = true;
            footstepAudio.Play();
        }
        else if (!isWalking && footstepAudio.isPlaying)
        {
            footstepAudio.Stop();
        }
    }


    void HandleJump()
    {
        if (!canMove || suppressJumpThisFrame)
        {
            suppressJumpThisFrame = false; // 한 프레임만 무시
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount < 2)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                jumpCount++;
                animator.SetTrigger("Jump");
            }
        }
    }


    void HandleAttack()
    {
        if (!canMove)
            return;

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            animator.SetTrigger("Attack2");
        }
    }

    public void DoAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<TutorialMonsterController>()?.TakeDamage();
        }
    }

    public void PlayAttackSound()
    {
        if (attackSfx == null || attackAudioSource == null)
        {
            Debug.LogWarning("공격 효과음 관련 컴포넌트 누락!");
            return;
        }

        attackAudioSource.PlayOneShot(attackSfx);
    }

    void UpdateAnimationParameters()
    {
        float speed = Mathf.Abs(rb.linearVelocity.x);
        int animState = speed > 0.1f ? 1 : 0;

        animator.SetInteger("AnimState", animState);
        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("AirSpeedY", rb.linearVelocity.y);
    }

    public void ShowPotionHintOnce()
    {
        if (hasShownPotionHint) return;

        hasShownPotionHint = true;

        if (uiManager != null)
        {
            uiManager.ShowPotionHint();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        PlayerStats.Instance.TakeDamage(damage);
        animator.SetTrigger("Hurt");

        // 피격 효과음 재생
        if (hitSfx != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(hitSfx);
        }

        if (PlayerStats.Instance.Health <= 0)
        {
            if (!isDead)  // 죽음 상태 한 번만 처리
            {
                isDead = true;  // 추가
                canMove = false;
                animator.SetTrigger("Death");

                // 죽는 효과음 재생
                if (deathSfx != null && sfxAudioSource != null)
                {
                    sfxAudioSource.PlayOneShot(deathSfx);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void HandleAttackHint()
    {
        if (attackHintDismissed) return;

        // Town_NPC2와 대화가 끝났고, 아직 힌트를 안 보여줬으면
        if (!hasShownAttackHint && DialogueManager.Instance.IsTownNPC2TalkCompleted)
        {
            hasShownAttackHint = true;
            uiManager?.ShowAttackHint();
        }

        // 힌트가 보이고 있고, Ctrl을 누르면
        if (hasShownAttackHint &&
            (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)))
        {
            uiManager?.HideAttackHint();
            attackHintDismissed = true;
        }
    }

}
