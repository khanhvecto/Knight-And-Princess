public class BleedEffect : ReusableEffect
{
    public override void ReleaseObject()
    {
        BleedEffectPool.Instance.Release(gameObject);
    }
}
