using UnityEngine;

public class SlimeAttackMove : MonoBehaviour
{
    //Reference
    [SerializeField] SlimeState stateScript;
    [SerializeField] SlimeCombat combatScript;
    [SerializeField] CombatStats statSO;
    [SerializeField] Rigidbody2D rb2D;
    [SerializeField] Animator animator;

    //Basic Movement
    private float distance; //Distance with human
    private float speed;

    //Tracker
    private Collider2D hostileColl;

    private void Start()
    {
        distance = statSO.distance;
        speed = statSO.combatSpeed;
    }

    public void Move()
    {
        //Re-update hostile object
        hostileColl = stateScript.targetColl;

        if(NeedToFlip())
        {
            stateScript.Flip();
        }
        
        //Check for move
        if(FarAwayHostile())
        {
            MoveToHostile();
        }
        else
        {
            Stop();
            combatScript.TryAttack();
        }
    }

    //Check if need to move to hostile

    private bool FarAwayHostile()
    {
        return Mathf.Abs(transform.position.x - hostileColl.transform.position.x) > distance;
    }
    private void MoveToHostile()
    {
        if (stateScript.facingLeft)
        {
            rb2D.velocity = new Vector2(-speed, rb2D.velocity.y);
        }
        else
        {
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        }
    }
    private void Stop()
    {
        rb2D.velocity = new Vector2(0, rb2D.velocity.y);
    }

    //Check if need to flip

    private bool NeedToFlip()
    {
        return (stateScript.facingLeft && transform.position.x < hostileColl.transform.position.x) ||
            (!stateScript.facingLeft && transform.position.x > hostileColl.transform.position.x);
    }
}
