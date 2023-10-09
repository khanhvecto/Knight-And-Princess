using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightState : MonoBehaviour, HpBarInterface
{
    private static KnightState instance;
    public static KnightState Instance { get => instance; }

    [Header("Reference")]
    [SerializeField] private LayerMask enemyLayer;
    public Rigidbody2D rb2D;
    private Animator animator;

    [Header("Fall back")]
    [SerializeField] public Vector2 fallBackVector;
    [SerializeField] public float fallBackForce;

    [Header("Combat stat")]
    [SerializeField] public float maxHealth;
    [SerializeField] public float health;
    [SerializeField] public float damage;
    [SerializeField] public float cooldown;
    [SerializeField] public float touchDamage;

    //Block enemy attack
    [HideInInspector] public bool blocking = false;
    [HideInInspector] public bool rightBlocking;    //Direction of blocking

    //Control state
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool controlable = true;

    private void Awake()
    {
        //Design pattern
        if (KnightState.instance != null) Debug.LogError("Only 1 KnightState allow to exist!");
        instance = this;

        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("alive", true);
    }

    public void flip() //Flip knight heading direction
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
        fallBackVector.x = -fallBackVector.x;
    }

    //Hurt state

    public void setFallBack()
    {
        this.controlable = false;
        rb2D.velocity = fallBackVector * fallBackForce;
    }

    public void StopControlable()
    {
        this.controlable = false;
    }

    public void SetControlable()
    {
        this.controlable = true;
    }

    public void setDead()
    {
        animator.SetBool("alive", false);

        //Set state
        this.controlable = false;
        gameObject.layer = 9;   //Dead layer

        //Stop motion
        rb2D.velocity = Vector2.zero;

        UIFunction.Instance.ShowDeadScreen(true);
    }

    public void setRespawn()
    {
        animator.SetBool("alive", true);
        this.controlable = true;
        gameObject.layer = 7;   //Knight layer

        //Reset stats
        this.health = this.maxHealth;

        UIFunction.Instance.ShowDeadScreen(false);

        //Reset camera
        CameraMovement.Instance.ResetDeadzone();
    }

    //Hp bar interface

    public float GetHp()
    {
        return this.health;
    }

    public float GetMaxHp()
    {
        return this.maxHealth;
    }

    //Draw on gizmos

    private void OnDrawGizmosSelected()
    {
        //Fall backward vector
        Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + fallBackVector);
    }
}
