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

    //State
    protected bool respawning = false;

    protected void Start()
    {
        this.respawnPlace = transform.Find("Left Ground").transform;
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 7) return;

        if(!this.respawning)
        {
            this.respawning = true;
            StartCoroutine(RespawnKnight());
        }
    }

    protected IEnumerator RespawnKnight()
    {
        //Set stats
        CameraMovement.Instance.followingKnight = false;

        //Player take fall damage;
        KnightHurt.Instance.TakeDamage(fallDamage);
            //Wait for animation
        yield return new WaitForSeconds(this.hurtTime);
            //Hide knight
        KnightState.Instance.gameObject.GetComponent<SpriteRenderer>().forceRenderingOff = true;

        //Check dead
        if(!KnightState.Instance.alive)
        {
            KnightState.Instance.gameObject.GetComponent<SpriteRenderer>().forceRenderingOff = false;
            CameraMovement.Instance.followingKnight = true;
            this.respawning = false;
            yield break;
        }

        //If the spawn point is far from hole (fall from high place), wait few secs before move camera
        if(Mathf.Abs(transform.position.y - this.respawnPlace.position.y) >= 4)
        {
            yield return new WaitForSeconds(this.waitTime);
        }

        //Focus camera to respawn point
        Vector3 spawnPos = new Vector3(this.respawnPlace.position.x, this.respawnPlace.position.y + CameraMovement.Instance.heightOffset, 0);
        //Vector3 spawnPos = new Vector3(this.respawnPlace.position.x, CameraMovement.Instance.verticalLevel + CameraMovement.Instance.heightOffset, 0);
        yield return StartCoroutine(CameraMovement.Instance.MoveToPos(spawnPos));

        //Respawn
        GamePlayLogic.Instance.TeleportKnight(this.respawnPlace);
        KnightState.Instance.gameObject.GetComponent<SpriteRenderer>().forceRenderingOff = false;

        //Set stats
        CameraMovement.Instance.ResetDeadzoneStats();
        this.respawning = false;
    }
}
