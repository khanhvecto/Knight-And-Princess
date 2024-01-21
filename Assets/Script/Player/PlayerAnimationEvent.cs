using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected PlayerMeleeAttack attack1Script;
    [SerializeField] protected PlayerMeleeAttack attack2Script;
    [SerializeField] protected PlayerMeleeAttack attack3Script;


    protected void Start()
    {
        this.LoadReferences();
    }

    protected void LoadReferences()
    {
        // Attack 1 script
        if (this.attack1Script == null)
            Debug.LogError("Can't find attack 1 script for PlayerAnimationEvent");
        // Attack 2 script
        if (this.attack2Script == null)
            Debug.LogError("Can't find attack 2 script for PlayerAnimationEvent");
        // Attack 3 script
        if (this.attack3Script == null)
            Debug.LogError("Can't find attack 3 script for PlayerAnimationEvent");
    }

    #region Attacks

    public void Attack1()
    {
        this.attack1Script.Attack();
    }
    
    public void Attack2()
    {
        this.attack2Script.Attack();
    }
    
    public void Attack3()
    {
        this.attack3Script.Attack();
    }

    #endregion
}
