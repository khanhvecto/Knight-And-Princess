using UnityEngine;

public class KnightBoss_AirAttack_Fall : StateMachineBehaviour
{
    // References
    protected KnightBossStats statsScript;
    protected Transform footPosition;
    protected LayerMask groundLayer;
    protected Animator animator;
    protected ParticleSystem blastEffect;
    protected MeleeAttack fallExplosion;

    // Stats
    protected float fallTime;
    protected float height;

    // States
    protected bool isLoadedReferences = false;
    protected bool playedBlastEffect = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!this.isLoadedReferences)
            this.LoadReferences(animator);

        this.playedBlastEffect = false;

        this.MakeFall();
    }

    protected void LoadReferences(Animator animator)
    {
        // Stats script
        this.statsScript = animator.GetComponentInChildren<KnightBossStats>();
        // foot position
        this.footPosition = animator.transform.Find("Movement").Find("Foot position");
        if (this.footPosition == null) Debug.LogError("Can't find foot position for KnightBoss_AirAttack_Jump of " + animator.name);
        // ground layer
        this.groundLayer = LayerMask.GetMask("Ground");
        // animator
        this.animator = animator;
        // blast effect
        this.blastEffect = animator.transform.Find("Effects").Find("Unique").Find("FallExplosion").GetComponent<ParticleSystem>();
        if (this.blastEffect == null) Debug.LogError("Can't find blast effect for KnightBoss_AirAttack_Jump of " + animator.name);
        // fall explosion
        this.fallExplosion = animator.transform.Find("Combat").Find("Skills").Find("AirAttack").Find("FallExplosion").GetComponentInChildren<MeleeAttack>();

        this.isLoadedReferences = true;
    }

    protected void FindHeight()
    {
        var hit = Physics2D.Raycast(this.footPosition.position, Vector2.down, 100, this.groundLayer.value);
        if (!hit) return;

        this.height = hit.distance;
    }

    protected void MakeFall()
    {
        if (this.statsScript.targetColl == null) return;

        this.FindHeight();

        this.fallTime = (float) this.animator.GetCurrentAnimatorStateInfo(0).length / 2;
        var direction = new Vector2(this.statsScript.targetColl.transform.position.x - animator.transform.position.x, -this.height);
        this.statsScript.rb2D.velocity = direction / fallTime;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // The boss stunned for 3 secs before active again
        if (this.playedBlastEffect) return;

        this.FindHeight();
        if (this.height <= 0.5)
        {
            this.playedBlastEffect = true;
            this.blastEffect.Play();
            this.fallExplosion.Attack();
        }
    }
}