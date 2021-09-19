using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 8;
    [SerializeField] float JumpForce = 200;

    void Update()
    {
        MoveHorizontal();
        Jump();
    }

    private void MoveHorizontal()
    {
        var horizontal = Input.GetAxis("Horizontal") * speed;
        var rigidbody2D = GetComponent<Rigidbody2D>();
        
        if (Mathf.Abs(horizontal) >= 1)
        {
            rigidbody2D.velocity = new Vector2(horizontal, rigidbody2D.velocity.y);
            //Adding a dollar sign allows to place variables in quotes
            //Debug.Log($"Velocity = {rigidbody2D.velocity}");
        }

        //Walk animation transition status
        var animator = GetComponent<Animator>();
        bool walking = horizontal != 0;
        animator.SetBool("Walk", walking);

        if (horizontal != 0)
        {
            //Turn Player animation to other direction
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = horizontal < 0;
        }
    }
    private void Jump()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpForce);
        }
    }
}
