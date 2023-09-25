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
    [SerializeField] private Animator animator;
    [SerializeField] private KnightCombat combatScript;
    private float speed = 8f;
    private float horizontal;
    private float jumpForce = 15f;
    private bool isFacingRight = true;
    private bool isGround;
    private bool controlable= true;

    [Header("Touch enemy fallback")]
    [SerializeField] private Vector2 fallBackVector;
    [SerializeField] private float fallBackForce;
    
    // Update is called once per frame
    void Update()
    {
        if (controlable)
        {
            checkGround();
            horizontal = Input.GetAxisRaw("Horizontal");
            //Flip 
            if ((horizontal < 0 && isFacingRight) || (horizontal > 0 && !isFacingRight)) flip();
            //Implement jump action
            if (Input.GetButtonDown("Jump") && isGround)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            }
            else if (Input.GetButtonUp("Jump") && rigidBody.velocity.y > 0)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
            else if (Input.GetButton("Jump") && isGround)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            }

            //Set parameters for animation
            if (horizontal != 0 && isGround) animator.SetBool("isRunning", true);
            else animator.SetBool("isRunning", false);
            if (!isGround && rigidBody.velocity.y > 0) animator.SetBool("isJumping", true);
            else animator.SetBool("isJumping", false);
            if (!isGround && rigidBody.velocity.y < 0) animator.SetBool("isFalling", true);
            else animator.SetBool("isFalling", false);
        }
        else if(rigidBody.velocity.y == 0)
        {
            controlable = true;
        }
    }

    private void FixedUpdate()
    {
        if (controlable)
        {
            rigidBody.velocity = new Vector2(horizontal * speed, rigidBody.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)    //Layer of enemy
        {
            gotHurt();
        }
    }

    private void gotHurt()
    {
        combatScript.touchEnemy();
        //Set fall back
        controlable = false;
        rigidBody.velocity = fallBackVector * fallBackForce;
        animator.SetTrigger("gotHurt");
    }

    private void flip() //Flip knight heading direction
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
        fallBackVector.x = -fallBackVector.x;
    }

    private void checkGround()
    {
        isGround = Physics2D.BoxCast(transform.position, boxGroundSize, 0f, Vector2.down, castBoxDistance, layerGround);
    }

    private void OnDrawGizmosSelected() //To show groundCheckBox
    {
        Gizmos.DrawWireCube((Vector2) transform.position + Vector2.down * castBoxDistance , boxGroundSize);
        //Fall backward vector
        Gizmos.DrawLine((Vector2) transform.position, (Vector2) transform.position + fallBackVector);
    }
}
