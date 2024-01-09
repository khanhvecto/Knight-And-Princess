using UnityEngine;

public class KnightBlock : MonoBehaviour
{
    //Design pattern
    private static KnightBlock instance;
    public static KnightBlock Instance { get => instance; }

    [Header("References")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        //Design pattern
        if (instance != null) Debug.LogError("Only 1 KnightBlock allow to exist!");
        instance = this;
    }

    void Start()
    {
        //Check references
        if (this.animator == null) Debug.LogError("Can't find animator for Knight Block");
    }

    void Update()
    {
        if(KnightState.Instance.controlable)
        {
            this.SetBlock();
        }

        this.SetEndurance();
    }

    private void SetBlock()
    {
        if (!KnightState.Instance.blocking && InputManager.Instance.GetBlockKeyDown())
        {
            this.StartBlock();
        }
        else if (KnightState.Instance.blocking && InputManager.Instance.GetBlockKey())
        {
            this.HoldingBlock();
        }
        else if (KnightState.Instance.blocking && !InputManager.Instance.GetBlockKey())
        {
            this.EndBlock();
        }
    }

    //
    //Block
    //
    private void StartBlock()
    {
        animator.SetTrigger("block");
        animator.SetBool("blocking", true);
        //Set blocking state
        KnightState.Instance.rb2D.velocity = Vector2.zero;
        KnightMovement.Instance.moveable = false;
        KnightState.Instance.blocking = true;
        //Set blocking direction
        this.SetBlockDirection();
        //Endurance
        KnightState.Instance.restoringEndurance = false;
    }
    private void HoldingBlock()
    {
        if(KnightState.Instance.controlable)
        {
            this.SetBlockDirection();
        }
    }
    private void EndBlock()
    {
        animator.SetBool("blocking", false);
        //Blocking state
        KnightMovement.Instance.moveable = true;
        KnightState.Instance.blocking = false;
        //Endurance
        KnightState.Instance.restoringEndurance = true;
    }

    private void SetBlockDirection()
    {
        if (KnightState.Instance.facingRight)
        {
            KnightState.Instance.rightBlocking = true;
        }
        else
        {
            KnightState.Instance.rightBlocking = false;
        }
    }

    //
    //Endurance
    //
    protected void SetEndurance()
    {
        //When using buff
        if (KnightState.Instance.indefatigable) return;

        //If not using buff
        if (KnightState.Instance.restoringEndurance)    //Restoring
        {
            if (KnightStats.Instance.endurance < KnightStats.Instance.maxEndurance)
            {
                this.RestoringEndurance(KnightStats.Instance.enduranceRestoreSpeed);
            }
        }
        else    //Loosing
        {
            if (KnightStats.Instance.endurance > KnightStats.Instance.minEndurance)
            {
                this.DecreasingEndurance(KnightStats.Instance.enduranceLooseSpeed);
            }
        }
    }
    protected void DecreasingEndurance(float value)
    {
        KnightStats.Instance.endurance -= value * Time.deltaTime;
        if (KnightStats.Instance.endurance < KnightStats.Instance.minEndurance)  //Limit: endurance can not smaller than min value
        {
            KnightStats.Instance.endurance = KnightStats.Instance.minEndurance;
        }
    }
    public void RestoringEndurance(float value)
    {
        KnightStats.Instance.endurance += value * Time.deltaTime;
        if (KnightStats.Instance.endurance > KnightStats.Instance.maxEndurance)  //Limit: endurance can not exceed max value
        {
            KnightStats.Instance.endurance = KnightStats.Instance.maxEndurance;
        }
    }
}
