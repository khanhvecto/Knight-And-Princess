using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("--- REFERENCES ---")]
    public Rigidbody2D rb2D;
    public Animator animator;
    [SerializeField] protected PlayerHUDBar HUDBarScript;

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
    [Header("Health")]
    public float maxHealth = 30;
    public float minHealth = 0;
    [SerializeField] protected float currentHealth;
    public float CurrentHealth { get => currentHealth; }
    [Header("Endurance")]
    public float maxEndurance = 100;
    public float minEndurance = 0;
    [SerializeField] protected float currentEndurance;
    public float CurrentEndurance { get => currentEndurance; }
    public float maxEnduranceRestoreSpeed = 5;
    public float currentEnduranceRestoreSpeed;
    public float enduranceDecreaseSpeed = 10;
    [Header("Damage")]
    public float damage = 5;
    [Header("Parry")]
    public float maxParryWindow = 0.2f;
    public float parryBuffer = 0.4f;
    [Header("Stunned")]
    public float stunTime = 3f;
    [Header("Attack")]
    public float pushForce = 50;
    public float pushTime = 0.1f;

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
    [Header("Endurance")]
    public bool enduranceRestoreable = true;
    public bool isShowingEnduranceBar = false;
    [Header("Stunned")]
    public bool stunnedable = true;
    [Header("Attack")]
    public bool attackable = true;
    public bool isAttacking = false;

    [Header("--- LAYERS ---")]
    public LayerMask groundLayerMask;
    public LayerMask enemyLayerMask;
    public int enemyLayer;
    public LayerMask deadLayerMask;
    public int deadLayer;

    protected void Start()
    {
        this.LoadReferences();
        this.InitStats();
    }

    protected void LoadReferences()
    {
        // rigid body 2D
        if (this.rb2D == null)
            Debug.LogError("Can't find rigid body 2D for Player_Stats of " + transform.parent.name);
        // animator
        if (this.animator == null)
            Debug.LogError("Can't find animator for Player_Stats of " + transform.parent.name);
        // HUD bar script
        if (this.HUDBarScript == null)
            Debug.LogError("Can't find HUD bar script for Player_Stats of " + transform.parent.name);
    }

    protected void InitStats()
    {
        // Layers
        this.enemyLayer = this.FindLayer(this.enemyLayerMask);
        this.deadLayer = this.FindLayer(this.deadLayerMask);

        // Init HUD bars
        this.HUDBarScript.InitHealthBar(this.minHealth, this.maxHealth);
        this.HUDBarScript.InitEnduranceBar(this.minEndurance, this.maxEndurance);

        // Init stats
        this.ResetCurrentStasts();
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

    #region Set stats

    public void ResetCurrentStasts()
    {
        this.SetCurrentHealthValue(this.maxHealth);
        this.SetCurrentEnduranceValue(this.maxEndurance);
    }

    public void SetCurrentHealthValue(float value)
    {
        if (value <= this.minHealth)
        {
            this.currentHealth = this.minHealth;
        }
        else if (value > this.maxHealth)
            this.currentHealth = this.maxHealth;
        else
            this.currentHealth = value;

        this.HUDBarScript.SetHealthBarValue(this.CurrentHealth);
    }

    public void SetCurrentEnduranceValue(float value)
    {
        // Set value
        if (value <= this.minEndurance)
        {
            this.currentEndurance = this.minEndurance;
        }
        else if (value > this.maxEndurance)
        {
            this.currentEndurance = this.maxEndurance;
        }
        else
        {
            this.currentEndurance = value;
        }

        // Update endurance bar
        this.HUDBarScript.SetEnduranceBarValue(this.CurrentEndurance);

        // Check to show bar
        if (this.CurrentEndurance < this.maxEndurance && !this.isShowingEnduranceBar)
        {
            this.HUDBarScript.ShowEnduranceBar(true);
            this.isShowingEnduranceBar = true;
        }
        else if (this.CurrentEndurance == this.maxEndurance && this.isShowingEnduranceBar)
        {
            this.HUDBarScript.ShowEnduranceBar(false);
            this.isShowingEnduranceBar = false;
        }
    }

    #endregion
}