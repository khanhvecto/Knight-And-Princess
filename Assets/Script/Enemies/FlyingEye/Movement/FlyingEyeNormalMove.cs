using UnityEngine;

public class FlyingEyeNormalMove: SlimeNormalMove
{
    [SerializeField] protected float defaultVerticalMoveRange;

    protected override void Start()
    {
        base.Start();

        this.defaultVerticalMoveRange = base.statSO.verticalMoveRange;
    }

    protected override void SetMove()
    {
        if (base.stateScript.facingLeft)
        {
            this.SetMoveVertical(-base.speed);
        }
        else
        {
            this.SetMoveVertical(base.speed);
        }
    }

    protected void SetMoveVertical(float horizontalSpeed)
    {
        if (Mathf.Abs(transform.position.y - base.spawnPlace.y) < defaultVerticalMoveRange)    // If still in vertical range
        {
            float rand = Random.Range(-speed / 5, speed / 5);
            rb2D.velocity = new Vector2(horizontalSpeed, rb2D.velocity.y + rand);
        }
        else
        {
            if (transform.position.y > base.spawnPlace.y)    // If enemy need to go lower
                rb2D.velocity = new Vector2(horizontalSpeed, -speed / 3);
            else                                            // If enemy need to go higher
                rb2D.velocity = new Vector2(horizontalSpeed, +speed / 3);
        }
    }
}