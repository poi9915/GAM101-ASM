using System;
using Interface;
using UnityEngine;

public class Plant : MonoBehaviour, IEnemy
{
    private static readonly int IsHit = Animator.StringToHash("isHit");
    private static readonly int IsTarget = Animator.StringToHash("isTarget");
    private static readonly int IsNormal = Animator.StringToHash("isNormal");

    [SerializeField] GameObject firePoint;
    [SerializeField] GameObject bullet;
    [SerializeField] float range = 10f; // The range for raycast
    [SerializeField] float fireRate = 0.25f; // Time in seconds between shots
    private Animator _animator;
    private float _gunHeat = 0f; // Cooldown timer
    private CapsuleCollider2D _collider2d;
    private Rigidbody2D _rb2d;
    private Vector2 _direction;
    private bool isDead = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2d = GetComponent<CapsuleCollider2D>();
        _rb2d = GetComponent<Rigidbody2D>();
        _direction = -transform.right;
        isDead = false;
    }

    void Update()
    {
        if (_gunHeat > 0)
        {
            _gunHeat -= Time.deltaTime;
        }

        RaycastHit2D hit =
            Physics2D.Raycast(firePoint.transform.position, _direction, range, LayerMask.GetMask("Player"));
        Debug.DrawRay(firePoint.transform.position, _direction * range, Color.red);

        if (hit.collider is not null)
        {
            //  Debug.Log("Player detected!");
            AttemptToShoot();
        }

        else
        {
            _animator.SetTrigger(IsNormal);
        }

        var localScale = transform.localScale;
        if (Mathf.Approximately(localScale.x, -1))
        {
            _direction = transform.right;
        }
        else if (Mathf.Approximately(localScale.x, 1))
        {
            _direction = -transform.right;
        }
    }
    //
    // public Vector2 GetDirection()
    // {
    //     return _direction;
    // }

    private void AttemptToShoot()
    {
        _animator.SetTrigger(IsNormal);
        // Only shoot if the gunHeat is 0 or less (cooldown complete)
        if (_gunHeat <= 0)
        {
            if (!isDead)
            {
                Shoot();
                _gunHeat = fireRate;
            }

        }
    }

    public void Hit()
    {
        _animator.SetTrigger(IsHit);
        isDead = true;
        transform.rotation = Quaternion.Euler(0, 0, -40);
        _rb2d.constraints = RigidbodyConstraints2D.None;
        _collider2d.enabled = false;
        Destroy(this.gameObject, 1.5f);
    }

    private void Shoot()
    {
        
        _animator.SetTrigger(IsTarget);
        // Instantiate(bullet, firePoint.transform.position, transform.rotation);
        GameObject bulletInstance = Instantiate(bullet, firePoint.transform.position, Quaternion.identity);
        Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
        float direction = transform.localScale.x > 0 ? -1 : 1;
        rb.velocity = new Vector2(direction * 5, 0);
        bulletInstance.transform.localScale = new Vector3(direction, 1, 1);
        Debug.Log("Shot fired!");
    }
}