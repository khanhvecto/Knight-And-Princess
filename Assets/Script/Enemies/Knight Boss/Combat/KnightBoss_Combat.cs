using UnityEngine;

public class KnightBoss_Combat : MonoBehaviour, IDamageReceiver
{
    [Header("References")]
    [SerializeField] protected KnightBossStats statScript;
    [SerializeField] protected Transform bleedPos;

    [Header("Stats")]
    protected int number_attacksTaken = 0;

    public void ChooseAttack()
    {
        // Choose a random attack
        int rand = Random.Range(1, this.statScript.attackTypeNumber + 1);
        this.statScript.animator.SetTrigger("attack" + rand);
    }

    #region Damage receiver

    public void GotHit(float damage, Transform attackPos, float enduranceDecrement)
    {
        this.number_attacksTaken++;
        this.statScript.SetHealthValue(this.statScript.Health - damage);
        this.PlayBleedEffect();
        this.PlayBloodSplashSound();
        this.statScript.SetAttackState(true);

        if (this.statScript.Health <= 0)
        {
            gameObject.layer = 9; //Dead layer
            this.statScript.animator.SetTrigger("dead");
            this.statScript.animator.SetBool("isDead", true);
        }
        else if (this.number_attacksTaken > this.statScript.unharmedAttacksAmount)
        {
            this.statScript.animator.SetTrigger("gotHit");
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
        if (this.statScript.facingLeft)
            effect.transform.Rotate(0f, 180f, 0f);
        effect.Play();
    }

    #endregion
}
