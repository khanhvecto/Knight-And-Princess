using System.Collections;
using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
    [Header("References")]
    public Transform respawnPlace;

    [Header("Stats")]
    [SerializeField] protected float hurtTime = 0.4f;
    [SerializeField] protected float waitTime = 1f; //If the hole is too high, then wait a few secs before move camera to spawn point
    [SerializeField] protected float fallDamage = 3f;

    [Header("Target")]
    protected Collider2D targetColl;
    protected PlayerStats playerStatsScript;
    protected PlayerManager playerManager;
    protected PlayerMovement playerMovementScript;

    //State
    protected bool respawning = false;

    protected void Start()
    {
        this.respawnPlace = transform.Find("Left Ground").transform;
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 7) return;

        this.targetColl = collision;
        this.playerStatsScript = collision.GetComponentInChildren<PlayerStats>();
        this.playerManager = collision.GetComponentInChildren<PlayerManager>();
        this.playerMovementScript = collision.GetComponentInChildren<PlayerMovement>();

        if (this.playerStatsScript.isDead)
            return;

        if(!CameraFollow.Instance.isFollowingPlayer)    // If camera is using for specific jobs
        {
            // Player take damage
            IDamageReceiver receiveDamageScript = this.targetColl.GetComponentInChildren<IDamageReceiver>();
            receiveDamageScript?.GotHit(this.fallDamage, transform.position, 0f);

            // Respawn
            this.playerManager.TeleportPlayer(this.respawnPlace);

            return;
        }

        if (!this.respawning)
        {
            this.respawning = true;
            StartCoroutine(RespawnKnight());
        }
    }

    protected IEnumerator RespawnKnight()
    {
        //Set stats
        CameraFollow.Instance.isFollowingPlayer = false;
        this.playerMovementScript.StopMoving();
        this.playerStatsScript.controlAbility = false;

        //Player take fall damage;
        IDamageReceiver receiveDamageScript = this.targetColl.GetComponentInChildren<IDamageReceiver>();
        receiveDamageScript?.GotHit(this.fallDamage, transform.position, 0f);
        //Wait for animation
        yield return new WaitForSeconds(this.hurtTime);
            //Hide knight
        this.targetColl.GetComponent<SpriteRenderer>().forceRenderingOff = true;

        //Check dead
        if(this.playerStatsScript.isDead)
        {
            this.targetColl.GetComponent<SpriteRenderer>().forceRenderingOff = false;
            CameraFollow.Instance.isFollowingPlayer = true;
            this.playerStatsScript.controlAbility = true;
            this.respawning = false;
            yield break;
        }

        //If the spawn point is far from hole (fall from high place), wait few secs before move camera
        if(Mathf.Abs(transform.position.y - this.respawnPlace.position.y) >= 4)
        {
            yield return new WaitForSeconds(this.waitTime);
        }

        //Focus camera to respawn point
        Vector3 spawnPos = new Vector3(this.respawnPlace.position.x, this.respawnPlace.position.y + CameraFollow.Instance.yAxisOffsetDefault, 0);
        yield return StartCoroutine(CameraFollow.Instance.MoveToPos(spawnPos));

        //Respawn
        this.playerManager.TeleportPlayer(this.respawnPlace);
        this.targetColl.GetComponent<SpriteRenderer>().forceRenderingOff = false;

        //Set stats
        CameraFollow.Instance.isFollowingPlayer = true;
        CameraFollow.Instance.ExitFreezeState();
        this.playerStatsScript.controlAbility = true;
        this.respawning = false;
    }
}
