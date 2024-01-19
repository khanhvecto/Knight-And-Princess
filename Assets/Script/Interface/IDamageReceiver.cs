using UnityEngine;

public interface IDamageReceiver
{
    void GotHit(float damage, Transform attackPos, float enduranceDecrement);
}
