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
    float _horizontal;

    //Components
    Rigidbody2D _rigidbody2D;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    bool _isGrounded;

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
        UpdateIsGrounded();
        ReadHorizontalInput();
        MoveHorizontal();

        UpdateAnimator();
        UpdateSpriteDirection();

        if (ShouldStartJump())
            Jump();
        else if (ShouldContinueJump())
            ContinueJump();

        _jumpTimer += Time.deltaTime;

        if (_isGrounded && _fallTimer > 0)
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

    void ContinueJump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpVelocity);
        _fallTimer = 0;
    }

    bool ShouldContinueJump()
    {
        return Input.GetButton("Fire1") && _jumpTimer <= _maxJumpsDuration;
    }

    void ReadHorizontalInput()
    {
        _horizontal = Input.GetAxis("Horizontal") * speed;
    }

    void MoveHorizontal()
    {
        if (Mathf.Abs(_horizontal) >= 1)
        {
            _rigidbody2D.velocity = new Vector2(_horizontal, _rigidbody2D.velocity.y);
            //Adding a dollar sign allows to place variables in quotes
            //Debug.Log($"Velocity = {rigidbody2D.velocity}");
        }
    }
    
    void UpdateAnimator()
    {
        //Walk animation transition status
        bool walking = _horizontal != 0;
        _animator.SetBool("Walk", walking);
    }

    void UpdateSpriteDirection()
    {
        if (_horizontal != 0)
        {
            //Turn Player animation to other direction
            _spriteRenderer.flipX = _horizontal < 0;
        }
    }

    void Jump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpVelocity);
        _jumpsRemaining--;
        Debug.Log($"Jumps remaining {_jumpsRemaining}");
        _fallTimer = 0;
        _jumpTimer = 0;
    }

    bool ShouldStartJump()
    {
        return Input.GetButtonDown("Fire1") && _jumpsRemaining > 0;
    }

    void UpdateIsGrounded()
    {
        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, LayerMask.GetMask("Default"));
        _isGrounded = hit != null;
    }

    //Kill Player
    internal void ResetToStart()
    {
        transform.position = _startPosition;
    }
}