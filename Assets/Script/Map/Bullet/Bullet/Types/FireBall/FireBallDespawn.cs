using UnityEngine;

public class FireBallDespawn : BulletDespawn
{
    public override void ReleaseObject()
    {
        FireBallPool.Instance.Release(this.gameObject);
    }
}
