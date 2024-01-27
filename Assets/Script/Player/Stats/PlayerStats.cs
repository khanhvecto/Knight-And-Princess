using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("--- REFERENCES ---")]
    public Rigidbody2D rb2D;
    public Animator animator;
    [SerializeField] protected PlayerHUDBar HUDBarScript;

    [Header("--- STATS ---")]
    [Header("Run")]
    public float speed;
    public float defaultSpeed = 4f;
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
    // Endurance restoration
    public float maxEnduranceRestoreSpeed = 5;
    public float currentEnduranceRestoreSpeed;
    // When player's printing
    public float enduranceDecreaseSpeed = 3;
    public float tiredTime = 3; // Endurance can stop restore if player's tired
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
    [Header("Look further")]

    [Header("--- LAYERS ---")]
    public LayerMask groundLayerMask;
    public LayerMask enemyLayerMask;
    public int enemyLayer;
    public LayerMask deadLayerMask;
    public int deadLayer;

    [Header("--- ABILITIES ---")]
    public bool controlAbility = true;
    public bool movementAbility = true;
    public bool sprintAbility = true;
    public bool rollAbiity = true;
    public bool blockAbility = true;
    public bool attackAbility = true;

    protected void Start()
    {
        this.InitStats();
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
        this.speed = this.defaultSpeed;
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

    #region Set combat stats

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

    #region Set sprint mode

    public void ChangeSprintMode()
    {
        if (this.isSprinting)
            this.SetSprintMode(false);
        else
            this.SetSprintMode(true);
    }

    public void SetSprintMode(bool state)
    {
        if (state == true)
        {
            this.speed = this.defaultSpeed * this.sprintCoef;
            this.isSprinting = true;
        }
        else
        {
            this.speed = this.defaultSpeed;
            this.isSprinting = false;
        }
    }

    #endregion
}