public class DangerInformEffect : ReusableEffect
{
    public override void ReleaseObject()
    {
        DangerInformPool.Instance.Release(gameObject);
    }
}
