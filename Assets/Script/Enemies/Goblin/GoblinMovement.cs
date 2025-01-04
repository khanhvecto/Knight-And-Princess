using UnityEngine;

public class GoblinMovement : KnightBossMove
{
    [Header("--- GOBLIN MOVEMENT ---")]
    [Header("References")]
    [SerializeField] protected Transform canvasObj;

    protected override void Flip()
    {
        base.Flip();
        this.canvasObj.transform.Rotate(0, 180, 0);
    }
}
