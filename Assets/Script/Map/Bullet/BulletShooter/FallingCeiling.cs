public class FallingCeiling : BulletShooter
{
    protected override void LoadBulletPool()
    {
        base.bulletPool = BrickPool.Instance;
    }
}
