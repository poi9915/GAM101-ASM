using System;
using Interface;
using UnityEngine;

public class Pig : MonoBehaviour, IEnemy
{
    private static readonly int IsTarget = Animator.StringToHash("isTarget");
    private static readonly int IsHit = Animator.StringToHash("isHit");

    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float runSpeed = 1.5f;
    [SerializeField] private float raycastDistance = 0.5f;
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    private Rigidbody2D _rb2d;
    private CapsuleCollider2D _col2d;
    private Animator _anim;
    private Transform _currentPoint;
    private bool _isDead = false;
    [SerializeField] private bool _isTarget = false;

    // Start is called before the first frame update
    private void Awake()
    {
        pointA.transform.SetParent(null);
        pointB.transform.SetParent(null);
    }

    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _col2d = GetComponent<CapsuleCollider2D>();
        _anim = GetComponent<Animator>();
        _currentPoint = pointA.transform;
    }

    private void Update()
    {
        if (!_isDead)
        {
            DetectingPlayer();
            _anim.SetBool(IsTarget, _isTarget);
        }
    }

    private void MoveBetweenPoint()
    {
        if (_currentPoint == pointB.transform)
        {
            _rb2d.velocity = new Vector2(walkSpeed, 0);
        }
        else
        {
            _rb2d.velocity = new Vector2(-walkSpeed, 0);
        }

        if (Vector2.Distance(transform.position, _currentPoint.position) < 0.5f)
        {
            Flip();
            _currentPoint = _currentPoint == pointA.transform ? pointB.transform : pointA.transform;
        }
    }

    private void DetectingPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            transform.localScale.x < 0 ? Vector2.right : Vector2.left,
            raycastDistance,
            LayerMask.GetMask("Player")
        );
        // RaycastHit2D wallHit = Physics2D.Raycast(
        //     transform.position,
        //     transform.localScale.x < 0 ? Vector2.right : Vector2.left,
        //     raycastDistance,
        //     LayerMask.GetMask("Player")
        // );

        if (hit.collider is not null)
        {
            Vector2 direction = hit.collider.transform.position - transform.position;
            _rb2d.velocity = new Vector2(Mathf.Sign(direction.x) * runSpeed, 0);
            _isTarget = true;
        }
        else
        {
            _isTarget = false;
            MoveBetweenPoint();
        }


        Debug.DrawRay(transform.position, (transform.localScale.x < 0 ? Vector2.right : Vector2.left) * raycastDistance,
            Color.red);
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void Hit()
    {
        _isDead = true;
        _anim.SetTrigger(IsHit);
        transform.rotation = Quaternion.Euler(0, 0, -40);
        _col2d.enabled = false;
        _rb2d.constraints = RigidbodyConstraints2D.None;
        Destroy(this.gameObject, 1.5f);
        Destroy(pointA, 2);
        Destroy(pointB, 2);
    }

    private void OnDrawGizmos()
    {
        if (!_isDead)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pointA.transform.position, 0.2f);
            Gizmos.DrawWireSphere(pointB.transform.position, 0.2f);
            Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        }
    }
}