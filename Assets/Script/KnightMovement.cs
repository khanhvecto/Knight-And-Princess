using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnightMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private Vector2 boxGroundSize;
    [SerializeField] private float castBoxDistance;
    private float speed = 8f;
    private float horizontal;
    private float jumpForce = 15f;
    //private bool isGround = false;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        //Jump
        if (Input.GetKeyDown(KeyCode.W) && isGround())
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        }
        if (Input.GetKeyUp(KeyCode.W) && rigidBody.velocity.y > 0) rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(horizontal * speed, rigidBody.velocity.y);
    }

    private bool isGround()
    {
        return Physics2D.BoxCast(transform.position, boxGroundSize, 0f, Vector2.down, castBoxDistance, layerGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube((Vector2) transform.position + Vector2.down * castBoxDistance , boxGroundSize);
    }
}
