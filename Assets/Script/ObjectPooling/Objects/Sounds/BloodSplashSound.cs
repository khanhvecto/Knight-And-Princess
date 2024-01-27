public class BloodSplashSound : ReusableSound
{
    public override void ReleaseObject()
    {
        BloodSplashSoundPool.Instance.Release(gameObject);
    }
}
