public class BleedEffect : ReusableEffect
{
    protected override void ReleaseObject()
    {
        BleedEffectPool.Instance.Release(gameObject);
    }
}
