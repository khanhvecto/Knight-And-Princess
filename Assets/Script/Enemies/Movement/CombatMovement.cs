using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMovement : MonoBehaviour
{
    //Reference
    [SerializeField] EnemyState stateScript;
    [SerializeField] CombatStats statSO;
    [SerializeField] Rigidbody2D rb2D;
    [SerializeField] Animator animator;

    //Basic Movement
    private float distance; //Distance with human
    private float speed;

    //Tracker
    private Collider2D hostileColl;

    //State
    public bool attackReady = false;

    private void Start()
    {
        distance = statSO.distance;
        speed = statSO.combatSpeed;
    }

    public void move()
    {
        //Re-update hostile object
        hostileColl = stateScript.targetColl;
        //Check if need to flip
        if(needToFlip())
        {
            stateScript.flip();
        }
        
        //Check for move
        if(farAwayHostile())
        {
            moveToHostile();
            attackReady = false;
        }
        else
        {
            stop();
            attackReady = true;
        }
    }

    //Check if need to move to hostile

    private bool farAwayHostile()
    {
        return Mathf.Abs(transform.position.x - hostileColl.transform.position.x) > distance;
    }
    private void moveToHostile()
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
    private void stop()
    {
        rb2D.velocity = new Vector2(0, rb2D.velocity.y);
    }

    //Check if need to flip

    private bool needToFlip()
    {
        return (stateScript.facingLeft && transform.position.x < hostileColl.transform.position.x) ||
            (!stateScript.facingLeft && transform.position.x > hostileColl.transform.position.x);
    }
}
