public class DangerInformEffect : ReusableEffect
{
    protected override void ReleaseObject()
    {
        DangerInformPool.Instance.Release(gameObject);
    }
}
