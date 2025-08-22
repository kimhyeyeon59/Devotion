using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float speed = 2f;
    public Transform leftPoint, rightPoint; // 좌우 이동 범위
    private bool movingRight = true;
    private Animator animator;
    private bool isTalking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isTalking)
        {
            Move();
        }

        animator.SetBool("isTalking", isTalking);
    }

    void Move()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (transform.position.x >= rightPoint.position.x)
                movingRight = false;

            GetComponent<SpriteRenderer>().flipX = false; // 오른쪽 볼 때
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (transform.position.x <= leftPoint.position.x)
                movingRight = true;

            GetComponent<SpriteRenderer>().flipX = true; // 왼쪽 볼 때
        }
    }


    public void StartTalking()
    {
        isTalking = true;
    }

    public void StopTalking()
    {
        isTalking = false;
    }
}
