using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{
    private static readonly int IsIsCollected = Animator.StringToHash("isCollected");
    private Animator animator;
    private readonly int _score = 1;
    private bool isCollected = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected)
        {
            if (other.CompareTag("Player"))
            {
                isCollected = true;
                GameManager.Instance.AddScore(_score);
                animator.SetTrigger(IsIsCollected);
                Destroy(this.gameObject, 1f);
            }
        }
    }
}