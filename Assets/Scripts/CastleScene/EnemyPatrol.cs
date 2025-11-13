using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 2f;
    public float moveDistance = 2f;

    [Header("시작 방향")]
    public bool startMovingRight = true;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingToTarget = true;
    private Animator animator;
    private bool hasWalkAnimation = false;
    private EnemyCombat combatScript;

    void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        combatScript = GetComponent<EnemyCombat>();

        // isWalking 파라미터 확인
        if (animator != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == "isWalking")
                {
                    hasWalkAnimation = true;
                    break;
                }
            }
        }

        // 초기 스프라이트 방향 정규화
        Vector3 scale = transform.localScale;
        if (startMovingRight)
        {
            scale.x = Mathf.Abs(scale.x); // 양수 (오른쪽)
            targetPosition = startPosition + new Vector3(moveDistance, 0, 0);
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x); // 음수 (왼쪽)
            targetPosition = startPosition + new Vector3(-moveDistance, 0, 0);
        }
        transform.localScale = scale;
    }

    void OnEnable()
    {
        // 다시 활성화될 때 애니메이션 설정
        if (hasWalkAnimation && animator != null)
            animator.SetBool("isWalking", true);
    }

    void Update()
    {
        // 전투 중이거나 죽었으면 이동 안 함
        if (combatScript != null && (combatScript.IsDead || combatScript.IsInCombat))
        {
            if (hasWalkAnimation && animator != null)
                animator.SetBool("isWalking", false);
            return;
        }

        // Walk 애니메이션 켜기
        if (hasWalkAnimation && animator != null)
            animator.SetBool("isWalking", true);

        if (movingToTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                movingToTarget = false;
                Flip();
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, startPosition) < 0.01f)
            {
                movingToTarget = true;
                Flip();
            }
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}