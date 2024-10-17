using Interface;
using UnityEngine;

public class EnemyControl : MonoBehaviour ,IEnemy
{
    //Property index
    private static readonly int IsTarget = Animator.StringToHash("isTarget");
    private static readonly int IsHit = Animator.StringToHash("isHit");
    private Rigidbody2D _rb2d;
    private BoxCollider2D _col2d;


    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private Vector2 detectionSize = new Vector2(2f, 5f);
    [SerializeField] private Transform playerTransform;
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private bool isDead = false;
    [SerializeField] private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();    
        _col2d = GetComponent<BoxCollider2D>();
    }


    private void FixedUpdate()
    {
        // Lock rotation
        _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (!isDead)
        {
            PlayerCheck();
        }

        if (isPlayer)
        {
            Move();
            anim.SetBool(IsTarget, isPlayer);
        }
        else
        {
            anim.SetBool(IsTarget, isPlayer);
        }
    }

    private void PlayerCheck()
    {
        // RaycastHit2D hit = Physics2D.Raycast
        // (
        //     transform.position, raycastDirection, raycastDistance, LayerMask.GetMask("Player")
        // );
        // Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);
        // isPlayer = hit.collider is not null;
        // RaycastHit2D hit = Physics2D.CircleCast(transform.position, detectionRadius, Vector2.zero, 0,
        //     LayerMask.GetMask("Player"));
        RaycastHit2D hit = Physics2D.BoxCast(
            new Vector2(transform.position.x, transform.position.y + 1),
            detectionSize,
            0f,
            Vector2.zero,
            0,
            LayerMask.GetMask("Player"));

        if (hit.collider is not null)
        {
            playerTransform = hit.transform;
            isPlayer = true;
        }
        else
        {
            isPlayer = false;
        }
    }

    private void Move()
    {
        Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (transform.position.x < targetPosition.x)
        {
            // Moving right: ensure the enemy is facing right (scale X positive)
            transform.localScale = new Vector2(-1, 1);
        }
        else if (transform.position.x > targetPosition.x)
        {
            // Moving left: flip the enemy (scale X negative)
            transform.localScale = new Vector2(1, 1);
        }
    }

    public void Hit()
    {
        isDead = true;
        isPlayer = false;
        anim.SetTrigger(IsHit);
        _rb2d.constraints = RigidbodyConstraints2D.None;
        transform.rotation = Quaternion.Euler(0, 0, -40);
        _col2d.enabled = false;
        Destroy(this.gameObject, 1.5f);
    }

    // Draw BoxCast
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y + 1), detectionSize);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if (other.gameObject.CompareTag($"Enemy"))
        // {
        //     ContactPoint2D contact = other.GetContact(1);
        //     if (contact.normal.y > 0.5f) 
        //     {
        //         Hit();
        //         Debug.Log("Get hit");
        //     }
        // }
        // Debug.Log("Va chạm với: " + other.gameObject.name);
        // if (other.contactCount > 0)
        // {
        //     ContactPoint2D contact = other.GetContact(0);
        //     Debug.Log("Normal của va chạm: " + contact.normal);
        // }
    }
}