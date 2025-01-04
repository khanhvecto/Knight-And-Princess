using UnityEngine;

public interface IDamageReceiver
{
    void GotHit(float damage, Vector3 attackPos, float enduranceDecrement);
}
