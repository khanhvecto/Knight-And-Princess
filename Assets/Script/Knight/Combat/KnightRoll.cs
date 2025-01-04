using UnityEngine;

public class KnightRoll : MonoBehaviour
{
    //Design pattern
    private static KnightRoll instance;
    public static KnightRoll Instance { get => instance; }

    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Stats")]
    [SerializeField] private float rollForce = 13f;
    private bool rollable = true;
    public bool rolling = false;

    private void Awake()
    {
        //Design pattern
        if (instance != null) Debug.LogError("Only 1 KnightRoll allows to exists");
        instance = this;
    }

    private void Start()
    {
        //Check references
        if (this.animator == null) Debug.LogError("Can't find animator for KnightRoll");
    }

    private void Update()
    {
        //If just stop roll then need to check if player can roll again
        if(!this.rolling && !this.rollable)
        {
            this.CheckRollable();
        }
        //If able to roll
        else if(this.rollable && InputManager.Instance.GetRollKeyDown() && KnightState.Instance.controlable)
        {
            animator.SetTrigger("roll");
        }
    }

    private bool CheckRollable()
    {
        if (KnightMovement.Instance.isGround)   //If the player in standing on ground, he can roll again
        {
            this.rollable = true;
            return true;
        }
        return false;
    }

    //Animator functions
    public void StartRoll()
    {
        this.rollable = false;
        this.rolling = true;
        KnightState.Instance.vulnerable = false;
        Physics2D.IgnoreLayerCollision(7, 8, true);   //Ignore collide of Knight and Enemy
        //Push body toward
        KnightState.Instance.controlable = false;
        KnightState.Instance.rb2D.gravityScale = KnightMovement.Instance.rollGravity;
        if (KnightState.Instance.facingRight)
        {
            KnightState.Instance.rb2D.velocity = Vector2.right * rollForce;
        }
        else
        {
            KnightState.Instance.rb2D.velocity = Vector2.left * rollForce;
        }
    }
    public void EndRoll()
    {
        this.rolling = false;
        KnightState.Instance.vulnerable = true;
        Physics2D.IgnoreLayerCollision(7, 8, false); //Reset collide of Knight and Enemy
        KnightState.Instance.controlable = true;
        if (KnightState.Instance.blocking)
        {
            KnightState.Instance.rb2D.velocity = Vector2.zero;
        }
        KnightState.Instance.rb2D.gravityScale = KnightMovement.Instance.defaultGravity;
    }
}
