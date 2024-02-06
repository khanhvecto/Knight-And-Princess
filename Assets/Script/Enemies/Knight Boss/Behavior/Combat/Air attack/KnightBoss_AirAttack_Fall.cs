using UnityEngine;

public class KnightBoss_AirAttack_Fall : StateMachineBehaviour
{
    [Header("References")]
    protected KnightBossStats statsScript;
    protected KnightBossMove movementScript;
    protected Transform footPosition;
    protected LayerMask groundLayer;
    protected Animator animator;
    protected ParticleSystem blastEffect;
    protected MeleeAttackCircle fallExplosion;
    protected float fallTime;

    [Header("Stats")]
    protected float height;
    protected float oldGravity;
    protected Vector2 momentum;
    protected int oldLayer;

    [Header("States")]
    protected bool isLoadedReferences = false;
    protected bool playedBlastEffect = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!this.isLoadedReferences)
            this.LoadReferences(animator);

        this.SetStats();
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
        this.fallExplosion = animator.transform.Find("Combat").Find("Skills").Find("AirAttack").Find("FallExplosion").GetComponentInChildren<MeleeAttackCircle>();
        this.movementScript = animator.GetComponentInChildren<KnightBossMove>();
        this.fallTime = (float) this.animator.GetCurrentAnimatorStateInfo(0).length / 2;

        this.isLoadedReferences = true;
    }

    protected void SetStats()
    {
        this.oldGravity = this.statsScript.rb2D.gravityScale;   // Gravity
        this.statsScript.rb2D.gravityScale = 0f;
        this.oldLayer = this.animator.gameObject.layer; // Layer
        this.animator.gameObject.layer = 9; // Dead layer
        this.playedBlastEffect = false; // Effect

        // Calculate velocity
        if (this.statsScript.targetColl == null) return;

        this.movementScript.CheckFlip();
        this.FindHeight();

        var direction = this.statsScript.targetColl.transform.position - animator.transform.position;
        Vector2 initialVelocity = 2 * (direction / this.fallTime);
        this.momentum = this.statsScript.rb2D.mass * initialVelocity;

        // Dash
        this.movementScript.DashForward(this.momentum);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // The boss stunned for 3 secs before active again
        if (this.playedBlastEffect) return;

        this.FindHeight();

        if (this.height <= 0.5)
        {
            this.ResetStats();
            this.playedBlastEffect = true;
            this.blastEffect.Play();
            this.fallExplosion.Attack();
            this.movementScript.StopMoving();
            CameraFollow.Instance.ShakeCamera(0.2f, 0.1f);
        }
    }
    protected void FindHeight()
    {
        var hit = Physics2D.Raycast(this.footPosition.position, Vector2.down, 100, this.groundLayer.value);
        if (!hit) return;

        this.height = hit.distance;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats();
    }

    protected void ResetStats()
    {
        this.statsScript.rb2D.gravityScale = this.oldGravity;
        this.animator.gameObject.layer = this.oldLayer;
    }
}