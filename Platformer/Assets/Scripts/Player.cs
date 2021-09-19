using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 1;

    void Update()
    {
        MoveHorizontal();
    }

    private void MoveHorizontal()
    {
        var horizontal = Input.GetAxis("Horizontal") * _speed;
        var rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = new Vector2(horizontal, rigidbody2D.velocity.y);

        //Adding a dollar sign allows to place variables in quotes
        Debug.Log($"Velocity = {rigidbody2D.velocity}");
    }
}
