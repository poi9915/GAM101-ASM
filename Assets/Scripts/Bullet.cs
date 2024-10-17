using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;

    void Update()
    {
        // transform.position += -transform.right * (speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground") || other.collider.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}