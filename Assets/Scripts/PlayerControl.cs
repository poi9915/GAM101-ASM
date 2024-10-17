using System;
using Interface;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    //Property index
    private static readonly int XVelocity = Animator.StringToHash("xVelocity");
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int IsDoubleJump = Animator.StringToHash("isDoubleJump");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int IsHit = Animator.StringToHash("isGetHit");
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float jumpForce = 5;
    [SerializeField] float fallMultiplier = 9.8f;
    [SerializeField] int jumpCount;
    [SerializeField] Animator animator;

    [SerializeField] private bool isGrounded = true;

    //[SerializeField] float wallSlidingSpeed = 5f;
    //[SerializeField] float wallJumpForce = 5;
    //[SerializeField] private bool isWallSliding = false;
    private Vector2 _inputVector;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _col2d;
    public bool isDead = false;


    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col2d = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        isDead = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }

        if (!isDead)
        {
            Move();
            // Lock rotation
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (_rb.velocity.y < 0)
            {
                _rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
            }

            animator.SetFloat(XVelocity, Math.Abs(_rb.velocity.x));
            animator.SetFloat(YVelocity, _rb.velocity.y);
            // if (_rb.velocity.y > 0)
            // {
            //     animator.SetTrigger(IsJumping);
            // }
            ////CheckWall();
            //if (isWallSliding && !isGrounded)
            //{
            //    rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            //    animator.SetTrigger("isWallJump");
            //}
        }
    }


    private void Move()
    {
        _rb.velocity = new Vector2(_inputVector.x * moveSpeed, _rb.velocity.y);

        if (_inputVector.x > 0)
        {
            // Moving right: ensure the player is facing right (scale X positive)
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_inputVector.x < 0)
        {
            // Moving left: flip the player (scale X negative)
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Jump()
    {
        if (isGrounded || jumpCount < 2)
        {
            if (jumpCount == 0)
            {
                animator.SetTrigger(IsJumping);
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            }
            else if (jumpCount == 1)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                animator.SetTrigger(IsDoubleJump);
            }

            jumpCount++;
        }
        //else if (isWallSliding)
        //{
        //    WallJump();
        //}
    }

    //private void WallJump()
    //{
    //    if (!isGrounded && (CheckWall() != 0))
    //    {
    //        animator.SetTrigger("isWallJump");
    //        // Determine wall direction
    //        int wallDirection = CheckWall();

    //        // Apply jump force away from the wall and upwards
    //        rb.velocity = new Vector2(wallDirection * wallJumpForce, jumpForce);

    //        // Flip the player in the opposite direction
    //        transform.localScale = new Vector3(wallDirection, 1, 1);

    //        // Reset jump counter to allow another jump after wall jump
    //        jumpCount = 0;

    //        // Set wall sliding to false after jumping off the wall
    //        isWallSliding = false;

    //        animator.SetTrigger("isWallJump");
    //    }
    //}

    //int CheckWall()
    //{
    //    RaycastHit2D hitRight = Physics2D.Raycast(transform.position , Vector2.right, 0.5f, LayerMask.GetMask("Wall"));
    //    RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.5f, LayerMask.GetMask("Wall"));
    //    Debug.DrawRay(transform.position, Vector2.right * 0.5f, Color.red);
    //    Debug.DrawRay(transform.position, Vector2.left * 0.5f, Color.red);
    //    if (!isGrounded)
    //    {
    //        if (hitRight.collider != null)
    //        {
    //            Debug.Log("Wall Right");
    //            isWallSliding = true;
    //            return 1; // Player is on the right wall
    //        }
    //        else if (hitLeft.collider != null)
    //        {
    //            Debug.Log("Wall Left");
    //            isWallSliding = true;
    //            return -1; // Player is on the left wall
    //        }
    //        else
    //        {
    //            isWallSliding = false;
    //            return 0; // No wall detected
    //        }
    //    }
    //    return 0;

    //}

    // Handle input system events//////////////////
    void OnMove(InputValue v)
    {
        _inputVector = v.Get<Vector2>();
    }

    void OnJump(InputValue v)
    {
        if (v.isPressed)
        {
            Jump();
        }
    }

    public void Hit()
    {
        Debug.Log("ur dead !!!!");
        animator.SetTrigger(IsHit);
        GameManager.Instance.GameOver();
    }
    ////////////////////////////////////////////////

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetTrigger(IsGrounded);
            jumpCount = 0;
            // isWallSliding = false;
            isGrounded = true;
        }

        //if(collision.gameObject.CompareTag("Wall"))
        //{
        //    isWallSliding = true;
        //}
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Bullet"))
        {
            Hit();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        //if (collision.gameObject.CompareTag("Wall"))
        //{
        //    isWallSliding = false;
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<IEnemy>();
            enemy?.Hit();
        }
    }
}