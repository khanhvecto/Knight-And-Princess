public class BloodSplashSound : ReusableSound
{
    protected override void ReleaseObject()
    {
        BloodSplashSoundPool.Instance.Release(gameObject);
    }
}
