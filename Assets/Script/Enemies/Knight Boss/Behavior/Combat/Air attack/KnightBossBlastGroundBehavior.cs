using UnityEngine;

public class KnightBossBlastGroundBehavior : StateMachineBehaviour
{
    // References
    protected KnightBossMove movementScript;
    protected ParticleSystem blastAppearing_ParticleSystem;
    protected ParticleSystem blast_ParticleSystem;
    protected GameObject blast_gameObject;

    // States
    protected bool isLoadedReferences = false;
    protected bool isPlayedBlastEffect = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences) 
            this.LoadReferences(animator);

        this.movementScript.StopMoving();
        this.ResetStats();
        this.OnEnterState();
    }

    protected void LoadReferences(Animator animator)
    {
        // movement script
        this.movementScript = animator.transform.Find("Movement").GetComponent<KnightBossMove>();
        if (this.movementScript == null)
            Debug.LogError("Can't find movement script for KnightBoss_BlastGround_Behavior of " + animator.name);
        // blast appearing particle system
        this.blastAppearing_ParticleSystem = animator.transform.Find("Effects").Find("Unique").Find("GroundBlast_Appearing").GetComponent<ParticleSystem>();
        if (this.blastAppearing_ParticleSystem == null)
            Debug.LogError("Can't find blast appearing particle system for KnightBoss_BlastGround_Behavior of " + animator.name);
        // blast particle system
        this.blast_ParticleSystem = animator.transform.Find("Effects").Find("Unique").Find("GroundBlast").GetComponent<ParticleSystem>();
        if (this.blast_ParticleSystem == null)
            Debug.LogError("Can't find blast particle system for KnightBoss_BlastGround_Behavior of " + animator.name);
        // blast gameObject
        this.blast_gameObject = animator.transform.Find("Combat").Find("Skills").Find("AirAttack").Find("BlastGround").gameObject;
        if (this.blast_gameObject == null)
            Debug.LogError("Can't find blast gameObject for KnightBoss_BlastGround_Behavior of " + animator.name);

        this.isLoadedReferences = true;
    }

    private void ResetStats()
    {
        this.isPlayedBlastEffect = false;
    }

    private void OnEnterState()
    {
        this.blastAppearing_ParticleSystem.Play();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (!this.blastAppearing_ParticleSystem.isStopped) return;

        if(!this.isPlayedBlastEffect)
        {
            this.blast_ParticleSystem.Play();
            CameraFollow.Instance.ShakeCamera(0.3f, 0.1f);
            this.blast_gameObject.SetActive(true);  // Send damage
            this.isPlayedBlastEffect = true;
            return;
        }

        if (!this.blast_ParticleSystem.isStopped) return;

        animator.SetTrigger("endState");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.blast_gameObject.SetActive(false);
        animator.ResetTrigger("endState");
    }
}
