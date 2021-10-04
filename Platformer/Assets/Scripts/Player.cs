using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 8;
    [SerializeField] float _jumpVelocity = 10;
    [SerializeField] int _maxJumps = 2;
    [SerializeField] Transform _feet;
    [SerializeField] float _downpull = 5;

    Vector3 _startPosition;
    int _jumpsRemaining;
    float _fallTimer;

    void Start()
    {
        _startPosition = transform.position;
        _jumpsRemaining = _maxJumps;
    }

    void Update()
    {
        

        MoveHorizontal();
        Jump();
    }

    void MoveHorizontal()
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

    void Jump()
    {
        var rigidbody2D = GetComponent<Rigidbody2D>();
        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, LayerMask.GetMask("Default"));
        bool isGrounded = hit != null;

        if (Input.GetButtonDown("Fire1") && _jumpsRemaining > 0)
        {
            
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, _jumpVelocity);
            _jumpsRemaining--;
            _fallTimer = 0;
        }

        if (isGrounded)
        {
            _fallTimer = 0;
            _jumpsRemaining = _maxJumps;
        } 
        else
        {
            _fallTimer += Time.deltaTime;
            var _downForce = _downpull * _fallTimer * _fallTimer;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y - _downForce);
        }
    }

    //Kill Player
    internal void ResetToStart()
    {
        transform.position = _startPosition;
    }
}
