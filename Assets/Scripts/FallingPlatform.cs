using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private static readonly int IsOff = Animator.StringToHash("isOFF");

    [SerializeField] private float fallDelay = 0.7f;
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private ParticleSystem destroyParticles;
    private Rigidbody2D _rb2d;
    private Animator _animator;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        _animator.SetBool(IsOff, true);
        destroyParticles.Stop();
        _rb2d.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyDelay);
    }
}