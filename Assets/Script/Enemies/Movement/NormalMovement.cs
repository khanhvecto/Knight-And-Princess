using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalMovement: MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Transform touchPoint;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyState stateScript;

    [Header("Layer")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Basic stats")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    private Vector2 spawnPlace;
    private float leftMoveRange;
    private float rightMoveRange;
    private bool isFacingLeft = true;

    [Header("Check wall stuck")]
    [SerializeField] private float checkWallRange;
    private bool isWallJumping = false;

    //The movement states are set up in animation behavior

    public void move()    //This is for the normal movement
    {
        float moveDistance = transform.position.x - spawnPlace.x;
        if (isWallJumping)
        {
            if (rb2D.velocity.y <= 0f)  //End of jump
            {
                if (!Physics2D.OverlapCircle(touchPoint.position, checkWallRange, groundLayer))    //Check if the wall is too high to jump
                {
                    //If the wall is jumpable over, then just jump over
                    moveHorizontal();
                }
                else
                {
                    //If the wall is too high,
                    //Reset range to make it never jump here again, just go into reverse
                    float distance = transform.position.x - spawnPlace.x;
                    if (Mathf.Abs(distance) <= 0.2)    //If the spawn place is too close, reset it away
                                                                                //the wall
                    {
                        if(isFacingLeft)
                        {
                            spawnPlace.x += 0.2f;
                            flip();
                            distance = transform.position.x - spawnPlace.x;
                            leftMoveRange = transform.position.x - spawnPlace.x;
                        }
                        else
                        {
                            spawnPlace.x -= 0.2f;
                            flip();
                            distance = transform.position.x - spawnPlace.x;
                            rightMoveRange = transform.position.x - spawnPlace.x;
                        }
                    }
                    //If the spawn is not too close, just reset the move range
                    else if (distance < 0)  
                    {
                        leftMoveRange = transform.position.x - spawnPlace.x;
                    }
                    else
                    {
                        rightMoveRange = transform.position.x - spawnPlace.x;
                    }
                }
                isWallJumping = false;
            }
        }
        else if (isStuck())  //Check if stucking at wall
        {
            //Check if stuck at wall
            if (Physics2D.OverlapCircle(touchPoint.position, checkWallRange, groundLayer))
            {
                Debug.Log("Enemy Stucking");
                //Jump only 1 time
                rb2D.velocity = new Vector2(0f, jumpForce);
                isWallJumping = true;
            }
            else    //If stuck by some other reasons
            {
                //Jump once
                rb2D.velocity = new Vector2(0f, jumpForce);
            }
        }
        else if ((moveDistance >= rightMoveRange && !isFacingLeft) ||
                (moveDistance <= leftMoveRange && isFacingLeft))    //If need to flip
        {
            flip();
        }
        else
        {
            moveHorizontal();
        }
    }


    void moveHorizontal()
    {
        if (isFacingLeft)
        {
            rb2D.velocity = new Vector2(-speed, rb2D.velocity.y);
        }
        else
        {
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        }
    }
    
    bool isStuck()
    {
        if (rb2D.velocity.x == 0 && rb2D.velocity.y == 0)
        {
            return true;
        }
        return false;
    }
    public void resetNormalMoveRange()
    {
        spawnPlace = transform.position;
        leftMoveRange = -1.5f;
        rightMoveRange = -leftMoveRange;
    }

    /*********************************************************
     *
     *********************************************************/

    void flip()
    {
        rb2D.velocity = new Vector2(-rb2D.velocity.x, rb2D.velocity.y);
        isFacingLeft = !isFacingLeft;
        rb2D.transform.Rotate(0f, 180f, 0f);
        stateScript.flip();
    }

    private void OnDrawGizmosSelected()
    {
        if (touchPoint != null)
        {
            Gizmos.DrawWireSphere(touchPoint.position, checkWallRange);
        }
    }
}
