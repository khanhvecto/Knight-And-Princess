public class PlayerBlockIdleBehavior : PlayerBlockBehavior
{
    protected override void SetStats()
    {
        base.statsScript.isBlocking = true;
        base.statsScript.movable = false;
    }
}
