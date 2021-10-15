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
    [SerializeField] float _maxJumpsDuration = 0.1f;

    Vector3 _startPosition;
    int _jumpsRemaining;
    float _fallTimer;
    float _jumpTimer;

    //Components
    Rigidbody2D _rigidbody2D;
    Animator _animator;
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _startPosition = transform.position;
        _jumpsRemaining = _maxJumps;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        MoveHorizontal();
        Jump();
    }

    void MoveHorizontal()
    {
        var horizontal = Input.GetAxis("Horizontal") * speed;
        
        if (Mathf.Abs(horizontal) >= 1)
        {
            _rigidbody2D.velocity = new Vector2(horizontal, _rigidbody2D.velocity.y);
            //Adding a dollar sign allows to place variables in quotes
            //Debug.Log($"Velocity = {rigidbody2D.velocity}");
        }

        //Walk animation transition status
        bool walking = horizontal != 0;
        _animator.SetBool("Walk", walking);

        if (horizontal != 0)
        {
            //Turn Player animation to other direction
            _spriteRenderer.flipX = horizontal < 0;
        }
    }

    void Jump()
    {
        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, LayerMask.GetMask("Default"));
        bool isGrounded = hit != null;

        if (Input.GetButtonDown("Fire1") && _jumpsRemaining > 0)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpVelocity);
            _jumpsRemaining--;
            Debug.Log($"Jumps remaining {_jumpsRemaining}");
            _fallTimer = 0;
            _jumpTimer = 0;
        }
        else if (Input.GetButton("Fire1") && _jumpTimer <= _maxJumpsDuration)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpVelocity);
            _fallTimer = 0;
        }

        _jumpTimer += Time.deltaTime;

        if (isGrounded && _fallTimer > 0)
        {
            _fallTimer = 0;
            _jumpsRemaining = _maxJumps;
        } 
        else
        {
            _fallTimer += Time.deltaTime;
            var _downForce = _downpull * _fallTimer * _fallTimer;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y - _downForce);
        }
    }

    //Kill Player
    internal void ResetToStart()
    {
        transform.position = _startPosition;
    }
}
