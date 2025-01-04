using UnityEngine;

public class GoblinEventAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected KnightBossMeleeAttackCircle attack1_1Script;
    [SerializeField] protected KnightBossMeleeAttackCircle attack1_2Script;
    [SerializeField] protected KnightBossMeleeAttackCircle attack1_3Script;
    [SerializeField] protected KnightBossMeleeAttackCircle attack2Script;

    #region Send damage

    public void Attack1()
    {
        this.attack1_1Script.Attack();
        this.attack1_2Script.Attack();
        this.attack1_3Script.Attack();
    }

    public void Attack2()
    {
        this.attack2Script.Attack();
    }

    #endregion
}
