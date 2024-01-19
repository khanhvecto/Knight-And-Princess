using UnityEngine;

public class KnightHurt : MonoBehaviour
{
    //Design pattern
    private static KnightHurt instance;
    public static KnightHurt Instance { get => instance; }

    [Header("References")]
    [SerializeField] private Animator animator;
    public Animator Animator { get => animator; }

    private void Awake()
    {
        //Design pattern
        if (instance != null) Debug.LogError("Only 1 KnightHurt allow to exist!");
        instance = this;
    }

    private void Start()
    {
        //Check references
        if (this.animator == null) Debug.LogError("Can't find animator for KnightHurt");
    }

    public void GotAttack(float damage, Transform attackPos, float enduranceDecrement)
    {
        //If using buff invincible 
        if (KnightState.Instance.invincible) return;

        if (KnightState.Instance.vulnerable)
        {
            //If not blocking
            if (!KnightState.Instance.blocking)
            {
                TakeDamage(damage, attackPos);
            }
            //If blocking
            else
            {
                //If bloking right and attacked left
                if (KnightState.Instance.rightBlocking && this.CheckLeftAttack(attackPos))
                {
                    KnightState.Instance.Flip();
                    this.TakeDamage(damage);
                }
                //If blocking left and attacked right
                else if (!KnightState.Instance.rightBlocking && !this.CheckLeftAttack(attackPos))
                {
                    KnightState.Instance.Flip();
                    this.TakeDamage(damage);
                }
                //If blocking toward enemy
                else
                {
                    //Decrease endurance
                    KnightStats.Instance.DecreaseEndurance(enduranceDecrement);
                    //Check if exhausted
                    if(KnightStats.Instance.endurance == KnightStats.Instance.minEndurance)
                    {
                        this.TakeDamage(damage);
                    }
                }
            }
        }
    }

    public void TakeDamage(float damage)    //Dont flip
    {
        this.animator.SetTrigger("gotHurt");
        
        if(!KnightState.Instance.invincible)
        {
            KnightStats.Instance.health -= damage;
        }

        KnightState.Instance.controlable = false;
        this.CheckDead();
    }

    public void TakeDamage(float damage, Transform attackPos)   //Can flip if not facing attackPos
    {
        //Check flip
        if(KnightState.Instance.facingRight && this.CheckLeftAttack(attackPos))
        {
            KnightState.Instance.Flip();
        }
        else if(!KnightState.Instance.facingRight && !this.CheckLeftAttack(attackPos))
        {
            KnightState.Instance.Flip();
        }

        //Take damage
        this.TakeDamage(damage);
    }

    public void CheckDead()
    {
        if (!KnightState.Instance.alive) return;

        if (KnightStats.Instance.health <= 0)
        {
            KnightState.Instance.SetDead();
        }
    }

    private bool CheckLeftAttack(Transform attackPos)
    {
        if (attackPos.transform.position.x < transform.parent.parent.position.x) return true;
        return false;
    }
}
