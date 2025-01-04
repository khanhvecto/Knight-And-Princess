using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightState : MonoBehaviour
{
    private static KnightState instance;
    public static KnightState Instance { get => instance; }

    [Header("Reference")]
    [SerializeField] private LayerMask enemyLayer = 1<<8;   //Combat layer
    public Rigidbody2D rb2D;
    private Animator animator;

    [Header("Fall back")]
    [SerializeField] public Vector2 fallBackVector = new Vector2(-0.77f, 1.26f);
    [SerializeField] public float fallBackForce = 6.15f;

    [Header("Block attack")]
    [SerializeField] public bool vulnerable = true;
    [SerializeField] public bool blocking = false;
    [SerializeField] public bool rightBlocking;    //Direction of blocking
    public bool restoringEndurance = true;

    [Header("Control state")]
    [SerializeField] public bool facingRight = true;
    [SerializeField] public bool controlable = true;
    [SerializeField] public bool alive = true;

    [Header("Buffs")]
    public bool invincible = false;    //Invincible buffs
    public bool indefatigable = false;  //Shield buffs

    private void Awake()
    {
        //Design pattern
        if (KnightState.instance != null) Debug.LogError("Only 1 KnightState allow to exist!");
        instance = this;
    }

    private void Start()
    {
        this.LoadReferences();

        animator.SetBool("alive", true);
    }
    private void LoadReferences()   //Check if any reference null
    {
        //Rigidbody 2D
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        if (this.rb2D == null) Debug.LogError("Can't find RigidBody2D for KnightState");
        //Animator
        animator = gameObject.GetComponent<Animator>();
        if (this.animator == null) Debug.LogError("Can't find animator for KnightState");
    }

    public void Flip() //Flip knight heading direction
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
        fallBackVector.x = -fallBackVector.x;
    }

    //Dead or alive state
    public void SetDead()
    {
        //Set state
        this.controlable = false;
        this.alive = false;
        gameObject.layer = 9;   //Dead layer

        //Stop motion
        rb2D.velocity = Vector2.zero;

        animator.SetBool("alive", false);
        animator.SetTrigger("dead");

        //UIFunction.Instance.ShowDeadScreen(true);
    }

    public void SetRespawn()
    {
        animator.SetBool("alive", true);
        this.controlable = true;
        this.vulnerable = true;
        this.alive = true;
        gameObject.layer = 7;   //Knight layer

        //Reset stats
        KnightStats.Instance.health = KnightStats.Instance.maxHealth;
        KnightStats.Instance.endurance = KnightStats.Instance.maxEndurance;
        KnightState.Instance.restoringEndurance = true;

        //UIFunction.Instance.ShowDeadScreen(false);

        //Reset camera
        CameraMovement.Instance.ResetDeadzone();
    }

    //Draw on gizmos
    private void OnDrawGizmosSelected()
    {
        //Fall backward vector
        Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + fallBackVector);
    }
}
