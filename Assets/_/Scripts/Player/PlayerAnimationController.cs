using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer renderer;
    private readonly int _running = Animator.StringToHash("Running");

    private void Update()
    {
        var velocity = rigidbody2D.velocity;

        if (velocity.x > 0f)
        {
            renderer.flipX = false;
        }
        else if (velocity.x < 0f)
        {
            renderer.flipX = true;
        }

        animator.SetBool(_running, velocity != Vector2.zero);
    }
}
