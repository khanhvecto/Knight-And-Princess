using UnityEngine;

public class FlyingEyeDeadBehavior : KnightBossDeadBehavior
{
    [Header("------ FLYING EYE DEAD BEHAVIOR ------")]
    [Header("Stats")]
    protected float deadGravity = 3;
    protected float oldGravity;

    protected override void SetStats()
    {
        base.SetStats();

        this.oldGravity = base.statsScript.rb2D.gravityScale;
        base.statsScript.rb2D.gravityScale = this.deadGravity;
    }

    protected override void ResetStats()
    {
        base.ResetStats();

        base.statsScript.rb2D.gravityScale = this.oldGravity;
    }
}
