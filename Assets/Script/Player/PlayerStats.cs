using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("--- REFERENCES ---")]
    public Rigidbody2D rb2D;
    public Animator animator;

    [Header("--- STATS ---")]
    [Header("Run")]
    public float speed = 4;
    public float sprintCoef = 1.5f;
    [Header("Jump")]
    public float jumpBuffer = 0.3f;
    public float coyoteTime = 0.2f;
    public float jumpForce = 12f;
    public float floatTime = 0.1f;
    public float stackJumpsAmount = 2;  // number of jumps can do in 1 shot
    [Header("Fall")]
    public float maxFallSpeed = 15;
    [Header("Roll")]
    public float rollCooldown = 1f;
    public float rollBuffer = 0.3f;
    public float rollForce = 13f;
    [Header("Combat")]
    public float health = 30;
    public float endurance = 100;
    public float damage = 5;

    [Header("--- STATES ---")]
    [Header("Run")]
    public bool movable = true;
    public bool isFacingRight = true;
    public bool isOnGround;
    [Header("Sprint")]
    public bool sprintable = true;
    public bool isSprinting = false;
    [Header("Control")]
    public bool controlable = true;
    [Header("Block")]
    public bool blockable = true;
    public bool isBlocking = false;
    [Header("Roll")]
    public bool rollable = true;
    [Header("Hurt")]
    public bool hurtable = true;
    [Header("Dead")]
    public bool isDead = false;

    [Header("--- LAYERS ---")]
    public LayerMask groundLayerMask;
    public LayerMask enemyLayerMask;
    public int enemyLayer;
    public LayerMask deadLayerMask;
    public int deadLayer;

    protected void Start()
    {
        this.LoadReferences();
        this.SetStats();
    }

    protected void LoadReferences()
    {
        // rigid body 2D
        if (this.rb2D == null)
            Debug.LogError("Can't find rigid body 2D for Player_Stats of " + transform.parent.name);
        // animator
        if (this.animator == null)
            Debug.LogError("Can't find animator for Player_Stats of " + transform.parent.name);
    }

    protected void SetStats()
    {
        this.enemyLayer = this.FindLayer(this.enemyLayerMask);
        this.deadLayer = this.FindLayer(this.deadLayerMask);
    }

    protected int FindLayer(LayerMask layerMask)
    {
        int counter = 0;
        while (layerMask > 1)
        {
            counter++;
            layerMask = layerMask >> 1;
        }
        return counter;
    }
}