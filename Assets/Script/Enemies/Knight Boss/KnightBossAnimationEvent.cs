using UnityEngine;

public class KnightBossAnimationEvent : MonoBehaviour
{
    [Header("--- REFERENCES ---")]
    [Header("MeleeCombo")]
    [SerializeField] protected KnightBossMeleeAttackCircle attack1;
    [SerializeField] protected KnightBossMeleeAttackCircle attack2;
    [SerializeField] protected KnightBossMeleeAttackBox attack3;
    [SerializeField] protected KnightBossMeleeAttackCircle attack4;

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
