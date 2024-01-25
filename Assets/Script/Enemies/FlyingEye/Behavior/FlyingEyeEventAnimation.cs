using UnityEngine;

public class FlyingEyeEventAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected KnightBossMeleeAttackCircle attack1Script;
    [SerializeField] protected KnightBossMeleeAttackCircle attack2Script;

    public void Attack1()
    {
        this.attack1Script.Attack();
    }

    public void Attack2()
    {
        this.attack2Script.Attack();
    }
}
