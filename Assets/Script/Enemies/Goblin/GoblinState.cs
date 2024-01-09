using UnityEngine;

public class GoblinState : SlimeState
{
    protected override void Start()
    {
        base.Start();

        base.facingLeft = false;
    }
}