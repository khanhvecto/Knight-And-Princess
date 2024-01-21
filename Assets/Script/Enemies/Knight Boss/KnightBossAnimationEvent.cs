using UnityEngine;

public class KnightBossAnimationEvent : MonoBehaviour
{
    [Header("--- REFERENCES ---")]
    [Header("MeleeCombo")]
    [SerializeField] protected KnightBossMeleeAttack attack1;
    [SerializeField] protected KnightBossMeleeAttack attack2;
    [SerializeField] protected KnightBossMeleeAttack attack3;
    [SerializeField] protected KnightBossMeleeAttack attack4;

    #region Melee combo

    public void Attack1()
    {
        this.attack1.Attack();
    }
    
    public void Attack2()
    {
        this.attack2.Attack();
    }
    
    public void Attack3()
    {
        this.attack3.Attack();
    }
    
    public void Attack4()
    {
        this.attack4.Attack();
    }

    #endregion
}
