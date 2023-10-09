using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnightMovement : MonoBehaviour
{
    private static KnightMovement instance;
    public static KnightMovement Instance { get => instance; }

    [Header("Reference")]
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private Vector2 boxGroundSize;
    [SerializeField] private float castBoxDistance;

    private Rigidbody2D rigidBody;
    private Animator animator;

    [Header("Stats")]
    [SerializeField] private float speed = 8f;
    public float Speed { get => speed; }
    [SerializeField] private float jumpForce = 15f;

    private float horizontal;
    public float Horizontal { get => horizontal; }
    private bool isGround;
    public bool IsGround { get => isGround; }

    private void Awake()
    {
        //Design pattern
        if (KnightMovement.instance != null) Debug.LogError("Only 1 KnightMovement allow to exist!");
        instance = this;

        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (KnightState.Instance.controlable)
        {
            checkGround();
            horizontal = Input.GetAxisRaw("Horizontal");
            //Flip 
            if ((horizontal < 0 && KnightState.Instance.facingRight) || (horizontal > 0 && !KnightState.Instance.facingRight)) KnightState.Instance.flip();
            //Implement jump action
            checkJump();

            //Set parameters for animation
            this.SetAnimation();
        }
    }

    private void FixedUpdate()
    {
        if (KnightState.Instance.controlable)
        {
            rigidBody.velocity = new Vector2(horizontal * speed, rigidBody.velocity.y);
        }
    }

    private void checkJump()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        }
        else if (Input.GetButtonUp("Jump") && rigidBody.velocity.y > 0)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.2f);
        else if (Input.GetButton("Jump") && isGround)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        }
    }

    private void SetAnimation()
    {
        if (horizontal != 0 && isGround) animator.SetBool("isRunning", true);
        else animator.SetBool("isRunning", false);
        if (!isGround && rigidBody.velocity.y > 0) animator.SetBool("isJumping", true);
        else animator.SetBool("isJumping", false);
        if (!isGround && rigidBody.velocity.y <= 0) animator.SetBool("isFalling", true);
        else animator.SetBool("isFalling", false);
    }

    private void checkGround()
    {
        //isGround = Physics2D.BoxCast(transform.position, boxGroundSize, 0f, Vector2.down, castBoxDistance, layerGround) &&
        //            rigidBody.velocity.y == 0;
        isGround = Physics2D.BoxCast(transform.position, boxGroundSize, 0f, Vector2.down, castBoxDistance, layerGround);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If touch enemy
        if (collision.gameObject.layer == 8)
        {
            Debug.Log("Touch");
            KnightHurt.Instance.TakeDamage(KnightState.Instance.touchDamage, collision.transform);
        }
    }

    private void OnDrawGizmosSelected() //To show groundCheckBox
    {
        //Draw checkground box
        Gizmos.DrawWireCube((Vector2) transform.position + Vector2.down * castBoxDistance , boxGroundSize);
    }
}
