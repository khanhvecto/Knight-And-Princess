using UnityEngine;

public class KnightBoss_Combat : MonoBehaviour, IDamageReceiver
{
    [Header("References")]
    [SerializeField] protected KnightBossStats statsScript;
    [SerializeField] protected Transform bleedPos;

    [Header("Stats")]
    protected int number_attacksTaken = 0;

    public void ChooseAttack()
    {
        // Choose a random attack
        int rand = Random.Range(1, this.statsScript.attackTypeNumber + 1);
        this.statsScript.animator.SetTrigger("attack" + rand);
    }

    #region Damage receiver

    public void GotHit(float damage, Vector3 attackPos, float enduranceDecrement)
    {
        if (this.statsScript.isDead)
            return;

        this.number_attacksTaken++;
        this.statsScript.SetHealthValue(this.statsScript.Health - damage);
        this.PlayBleedEffect();
        this.PlayBloodSplashSound();
        this.statsScript.SetAttackState(true);

        if (this.statsScript.Health <= 0)
        {
            this.statsScript.animator.SetTrigger("dead");
            this.statsScript.animator.SetBool("isDead", true);
        }
        else if (this.number_attacksTaken > this.statsScript.unharmedAttacksAmount)
        {
            this.statsScript.animator.SetTrigger("gotHit");
            this.number_attacksTaken = 0;   // Reset
        }
    }

    protected void PlayBloodSplashSound()
    {
        // the sound is played on awake, so don't need to play it manually
        BloodSplashSoundPool.Instance.Get();
    }

    protected void PlayBleedEffect()
    {
        ParticleSystem effect = BleedEffectPool.Instance.Get().GetComponent<ParticleSystem>();
        if (effect == null)
            return;

        // Put the effect at right position
        effect.transform.position = this.bleedPos.position;
        if (this.statsScript.facingLeft)
            effect.transform.Rotate(0f, 180f, 0f);
        effect.Play();
    }

    #endregion

    public void Revive()
    {
        this.statsScript.animator.SetBool("isDead", false);
    }
}
