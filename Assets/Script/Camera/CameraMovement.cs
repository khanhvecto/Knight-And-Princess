using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //Design pattern
    private static CameraMovement instance;
    public static CameraMovement Instance { get => instance; }

    [Header("References")]
    [SerializeField] private Transform knightPos;
    [Header("Speed")]
    [SerializeField] private float followSpeed = 3f;  //Speed when player running stable
    [SerializeField] private float localSpeed = 1f;   //Speed when in deadzone
    [Header("Offset")]
    [SerializeField] private float heightOffset = 1f;
    [SerializeField] private float widthOffset = 5f;
    private float widthRange;
    private float zAxisPosition = -10f;
    [Header("Dead zone")]
    [SerializeField] private float deadZoneHorizontal = 3f;
    [SerializeField] private float deadZoneVertical = 3f;
    private bool inDeadZone = true;
        //Deadzone facing counter
    private float deadZoneMoveTimer = 0f;
    private bool deadZoneMoveFlag = false;
    private bool oldFacingRight;
        //Deadzone border
    private Vector3 deadZonePos;
    private float leftDeadBorder;
    private float rightDeadBorder;
    private float topDeadBorder;
    private float bottomDeadBorder;
        [Header("Looking further")]
    [SerializeField] private float lookHorizontalRange = 6f;
    [SerializeField] private float lookVerticalRange = 3f;

    private void Awake()
    {
        //Design pattern
        if (instance != null) Debug.LogError("Only 1 CameraMovement allow to exist!");
        instance = this;

        //Start position
        this.ResetToKnightPos();

        //Dead zone
        this.SetDeadZoneRange();
        this.oldFacingRight = KnightState.Instance.facingRight;
    }

    private void Update()
    {
        this.MoveCamera();
    }

    private void MoveCamera()
    {
        if (this.inDeadZone)
        {
            this.DeadZoneMove();
        }
        else
        {
            this.RunningMove();
        }
    }

    /// <summary>
    /// Move when in deadzone
    /// </summary>
    private void DeadZoneMove()
    {
        if (!this.SetLookFurther()) //If not looking further, then move to steady state
        {
            this.SetDeadZoneHeading();
            Vector3 newPos = new Vector3(this.deadZonePos.x + this.widthRange / 2, this.deadZonePos.y + this.heightOffset, this.zAxisPosition);
            transform.position = Vector3.Slerp(transform.position, newPos, this.localSpeed * Time.deltaTime);
        }

        if (this.OutOfDeadZone())   //Check if out of deadzone
        {
            this.RunningMove();
            this.inDeadZone = false;
        }
    }

    private void SetDeadZoneHeading()
        //Set the head of deadzone that only change when in a heading state for amount of time
    {
        if (this.oldFacingRight != KnightState.Instance.facingRight)
        {
            this.deadZoneMoveTimer = 0f;
            this.oldFacingRight = KnightState.Instance.facingRight;
            this.deadZoneMoveFlag = false;
        }
        else if (this.deadZoneMoveFlag == false)
        {
            if (this.deadZoneMoveTimer < 0.3f)  //Means it takes 0.3s stand still to change direction
            {
                this.deadZoneMoveTimer += Time.deltaTime;
            }
            else
            {
                this.CheckHeading();
                this.deadZoneMoveFlag = true;
            }
        }
    }

    private bool SetLookFurther()   
        //Check from input, if player looking further then set camera move and return true
    {
        if (InputManager.Instance.GetLookDownKey())
        {
            if (InputManager.Instance.GetLookRightKey())
                //Look bottom-right
            {
                Vector3 newPos = new Vector3(this.knightPos.position.x + this.lookHorizontalRange, this.knightPos.position.y - this.lookVerticalRange, this.zAxisPosition);
                transform.position = Vector3.Slerp(transform.position, newPos, this.localSpeed * Time.deltaTime);
            }
            else if (InputManager.Instance.GetLookLeftKey())
                //Look bottom-left
            {
                Vector3 newPos = new Vector3(this.knightPos.position.x - this.lookHorizontalRange, this.knightPos.position.y - this.lookVerticalRange, this.zAxisPosition);
                transform.position = Vector3.Slerp(transform.position, newPos, this.localSpeed * Time.deltaTime);
            }
            else
                //Look bottom only
            {
                Vector3 newPos = new Vector3(this.knightPos.position.x, this.knightPos.position.y - this.lookVerticalRange, this.zAxisPosition);
                transform.position = Vector3.Slerp(transform.position, newPos, this.localSpeed * Time.deltaTime);
            }
            return true;
        }
        else if (InputManager.Instance.GetLookUpKey())
        {
            if(InputManager.Instance.GetLookRightKey())
                //Look top right
            {
                Vector3 newPos = new Vector3(this.knightPos.position.x + this.lookHorizontalRange, this.knightPos.position.y + this.lookVerticalRange, this.zAxisPosition);
                transform.position = Vector3.Slerp(transform.position, newPos, this.localSpeed * Time.deltaTime);
            }
            else if(InputManager.Instance.GetLookLeftKey())
                //Look top left
            {
                Vector3 newPos = new Vector3(this.knightPos.position.x - this.lookHorizontalRange, this.knightPos.position.y + this.lookVerticalRange, this.zAxisPosition);
                transform.position = Vector3.Slerp(transform.position, newPos, this.localSpeed * Time.deltaTime);
            }
            else
                //Look top only
            {
                Vector3 newPos = new Vector3(this.knightPos.position.x, this.knightPos.position.y + this.lookVerticalRange, this.zAxisPosition);
                transform.position = Vector3.Slerp(transform.position, newPos, this.localSpeed * Time.deltaTime);
            }
            return true;
        }
        else if (InputManager.Instance.GetLookRightKey())
            //Look right only
        {
            Vector3 newPos = new Vector3(this.knightPos.position.x + this.lookHorizontalRange, this.knightPos.position.y + this.heightOffset, this.zAxisPosition);
            transform.position = Vector3.Slerp(transform.position, newPos, this.localSpeed * Time.deltaTime);
            return true;
        }
        else if (InputManager.Instance.GetLookLeftKey())
            //Look left only
        {
            Vector3 newPos = new Vector3(this.knightPos.position.x - this.lookHorizontalRange, this.knightPos.position.y + this.heightOffset, this.zAxisPosition);
            transform.position = Vector3.Slerp(transform.position, newPos, this.localSpeed * Time.deltaTime);
            return true;
        }

        return false;
    }

    private bool OutOfDeadZone()
    {
        if (this.knightPos.position.x >= this.rightDeadBorder || this.knightPos.position.x <= this.leftDeadBorder ||
            this.knightPos.position.y >= this.topDeadBorder || this.knightPos.position.y <= this.bottomDeadBorder)
        {
            return true;
        }
        return false;
    }
    private void SetDeadZoneRange()
    {
        this.deadZonePos = this.knightPos.position;
        this.leftDeadBorder = this.deadZonePos.x - this.deadZoneHorizontal;
        this.rightDeadBorder = this.deadZonePos.x + this.deadZoneHorizontal;
        this.topDeadBorder = this.deadZonePos.y + this.deadZoneVertical;
        this.bottomDeadBorder = this.deadZonePos.y - this.deadZoneVertical;
    }

    public void ResetDeadzone() //For restart camera at deadzone in general
    {
        this.inDeadZone = true;
        this.SetDeadZoneRange();
        this.oldFacingRight = KnightState.Instance.facingRight;
    }

    /// <summary>
    /// Move when running
    /// </summary>
    private void RunningMove()
    {
        //Follow
        this.CheckHeading();
        Vector3 newPosition = new Vector3(knightPos.position.x + widthRange, knightPos.position.y + heightOffset, zAxisPosition);
        transform.position = Vector3.Slerp(transform.position, newPosition, (this.followSpeed) * Time.deltaTime);

        //Check if standing
        if (KnightMovement.Instance.Horizontal == 0f)
        {
            this.ResetDeadzone();
        }
    }

    /// <summary>
    /// Other
    /// </summary>
    private void CheckHeading()
    {
        if (KnightState.Instance.facingRight)
        {
            this.widthRange = Mathf.Abs(this.widthOffset);
        }
        else
        {
            this.widthRange = -Mathf.Abs(this.widthOffset);
        }
    }
    public void ResetToKnightPos()
    {
        Vector2 startPos = this.knightPos.position;
        this.transform.position = new Vector3(startPos.x, startPos.y, this.zAxisPosition);
    }
}
