using UnityEngine;

public class SlimeDamageReceiver: MonoBehaviour, IDamageReceiver
{
    [Header("References")]
    [SerializeField] protected SlimeState stateScript;
    [SerializeField] protected SlimeStats statScript;
    // effect
    [SerializeField] protected ParticleSystem bleed_particleSystem;
    // audio
    [SerializeField] protected string bloodSplashSoundPoolTag = "BloodSplashSoundPool";
    protected ObjectPooling bloodSplashSoundPoolScript;

    [Header("Stats")]
    protected int number_attacksTaken = 0;

    private void Start()
    {
        this.CheckReferences();
    }

    private void CheckReferences()   //Check if any reference not found
    {
        //EnemyStats
        if (this.statScript == null) 
            Debug.LogError("Can't find EnemyStats for SlimeDamageReceiver of " + transform.parent.name);
        //EnemyState
        if (this.stateScript == null) 
            Debug.LogError("Can't find EnemyState for SlimeDamageReceiver of " + transform.parent.name);
        // bleed particle system
        if (this.bleed_particleSystem == null)
            Debug.LogError("Can't find bleed particle system for SlimeDamageReceiver of " + transform.parent.name);
        // blood splash sound pool script
        GameObject bloodSplashSoundPoolObj = GameObject.FindWithTag(this.bloodSplashSoundPoolTag);
        if (bloodSplashSoundPoolObj == null)
            Debug.LogError("Can't find blood splash sound pool object for SlimeDamageReceiver of " + transform.parent.name);
        else
        {
            this.bloodSplashSoundPoolScript = bloodSplashSoundPoolObj.GetComponent<ObjectPooling>();
            if (this.bloodSplashSoundPoolScript == null)
                Debug.LogError("Can't find blood splash sound pool script for SlimeDamageReceiver of " + transform.parent.name);
        }
    }

    //Got hurt
    public void GotHit(float damage, Transform attackPos, float enduranceDecrement)
    {
        this.number_attacksTaken++;
        this.statScript.health -= damage;
        this.bleed_particleSystem.Play();
        this.PlayBloodSplashSound();
        stateScript.SetAttackState(true);

        if (this.number_attacksTaken > this.statScript.unharmedAttacksAmount)
        {
            this.stateScript.animator.SetTrigger("gotHit");
            this.number_attacksTaken = 0;   // Reset
        }

        if (this.statScript.health <= 0)
        {
            gameObject.layer = 9; //Dead layer
            this.stateScript.animator.SetTrigger("dead");
            this.stateScript.animator.SetBool("isDead", true);
        }
    }

    protected void PlayBloodSplashSound()
    {
        // the sound is played on awake, so don't need to play it manually
        this.bloodSplashSoundPoolScript.Get();
    }
}