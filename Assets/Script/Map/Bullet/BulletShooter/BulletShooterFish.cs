using UnityEngine;

public class BulletShooterFish : BulletShooter
{ 
    protected override void LoadBulletPool()
    {
        base.bulletPool = FireBallPool.Instance;
    }
}
