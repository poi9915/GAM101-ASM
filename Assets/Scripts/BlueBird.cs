using System.Collections;
using System.Collections.Generic;
using Interface;
using UnityEngine;

public class BlueBird : MonoBehaviour, IEnemy
{
    private static readonly int IsHit = Animator.StringToHash("isHit");
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    [SerializeField] private float speed;
    [SerializeField] private bool _isDead = false;
    private Rigidbody2D _rb2d;
    private BoxCollider2D _box2d;
    private Animator _anim;
    private Transform _currentPoint;

    // Start is called before the first frame update
    private void Awake()
    {
        pointA.transform.SetParent(null);
        pointB.transform.SetParent(null);
    }

    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _box2d = GetComponent<BoxCollider2D>();
        _anim = GetComponent<Animator>();
        _currentPoint = pointA.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDead)
        {
            MoveBetweenPoint();
        }
    }

    private void MoveBetweenPoint()
    {
        if (_currentPoint == pointB.transform)
        {
            _rb2d.velocity = new Vector2(speed, 0);
        }
        else
        {
            _rb2d.velocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, _currentPoint.position) < 0.5f)
        {
            Flip();
            _currentPoint = _currentPoint == pointA.transform ? pointB.transform : pointA.transform;
        }
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
        _box2d.enabled = false;
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