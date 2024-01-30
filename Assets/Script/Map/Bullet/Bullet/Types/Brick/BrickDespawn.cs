using UnityEngine;

public class BrickDespawn : BulletDespawn
{
    public override void ReleaseObject()
    {
        BrickPool.Instance.Release(gameObject);
    }

    protected override void RotateBullet(Vector2 newDirection)
        // Randomly rotate the brick
    {
        float randomAngle = Random.Range(-180, 180);

        transform.Rotate(0f, 0f, randomAngle);
    }
}