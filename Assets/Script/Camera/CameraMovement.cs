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
    [SerializeField] protected GameObject UIObj;
    [SerializeField] protected CanvasGroup fadeScreenCanvas;

    [Header("State")]
    [SerializeField] protected bool followingKnight = true;

    [Header("Speed")]
    [SerializeField] private float followSpeed = 4f;  //Speed when player running stable
    [SerializeField] private float localSpeed = 1f;   //Speed when in deadzone

    //Vertical level
    protected float verticalLevel;
    protected bool verticalMoveHard;
    protected Vector3 relativePos;  //When player move too fast, the camera will be "freeze"

    [Header("Offset")]
    [SerializeField] private float heightOffset = 2f;
    [SerializeField] private float widthOffset = 5f;
    private float widthRange;
    private float zAxisPosition = -10f;

    [Header("Dead zone")]
    [SerializeField] private float deadZoneHorizontal = 3f;
    [SerializeField] private float deadZoneVertical = 2f;
    private bool inDeadZone = true;
        //Deadzone facing counter
    private float deadZoneMoveTimer = 0f;
    private bool deadZoneMoveFlag = false;
    private bool oldFacingRight;
        //Deadzone border
    private Vector3 deadZonePos;
    private float leftDeadBorder;
    private float rightDeadBorder;
        [Header("Looking further")]
    [SerializeField] private float lookHorizontalRange = 6f;
    [SerializeField] private float lookVerticalRange = 3f;

    private void Awake()
    {
        //Design pattern
        if (instance != null) Debug.LogError("Only 1 CameraMovement allow to exist!");
        instance = this;

        //Check references
        this.LoadReferences();
    }
    protected void LoadReferences()
    {
        //knight position
        if (this.knightPos == null) Debug.LogError("Can't find knight position for main camera");
        //UI Object
        if (this.UIObj == null) Debug.LogError("Can't find UI object for main camera");
        //Fade screen CanvasGroup
        if (this.fadeScreenCanvas == null) Debug.LogError("Can't find fade screen CanvasGroup for main camera");
    }

    private void Start()
    {
        //Start position
        this.ResetToKnightPos();

        //Dead zone
        this.SetDeadZoneRange();
        this.oldFacingRight = KnightState.Instance.facingRight;
    }

    private void Update()
    {
        if(this.followingKnight) this.MoveCamera();
    }

    private void MoveCamera()
    {
        //Find vertical level
        this.FindVerticalLevel();

        //Set move
        if(!this.verticalMoveHard)
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
        else
        {
            Vector3 tmpPos = this.knightPos.position + this.relativePos;
            transform.position = new Vector3(tmpPos.x, tmpPos.y, this.zAxisPosition);
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
            Vector3 newPos = new Vector3(this.deadZonePos.x + this.widthRange / 2, this.verticalLevel + this.heightOffset, this.zAxisPosition);
            Vector3 tmpPos_1 = Vector3.Slerp(transform.position, newPos, this.localSpeed * Time.deltaTime);
            Vector3 tmpPos_2 = Vector3.Slerp(transform.position, newPos, this.followSpeed * Time.deltaTime);
            transform.position = new Vector3(tmpPos_1.x, tmpPos_2.y, this.zAxisPosition);
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
                Vector3 newPos = new Vector3(this.deadZonePos.x + this.widthRange/2 + this.lookHorizontalRange, this.knightPos.position.y - this.lookVerticalRange, this.zAxisPosition);
                transform.position = Vector3.Slerp(transform.position, newPos, this.followSpeed * Time.deltaTime);
            }
            else if (InputManager.Instance.GetLookLeftKey())
                //Look bottom-left
            {
                Vector3 newPos = new Vector3(this.deadZonePos.x + this.widthRange / 2 - this.lookHorizontalRange, this.knightPos.position.y - this.lookVerticalRange, this.zAxisPosition);
                transform.position = Vector3.Slerp(transform.position, newPos, this.followSpeed * Time.deltaTime);
            }
            else
                //Look bottom only
            {
                Vector3 newPos = new Vector3(this.deadZonePos.x + this.widthRange / 2, this.knightPos.position.y - this.lookVerticalRange, this.zAxisPosition);
                transform.position = Vector3.Slerp(transform.position, newPos, this.followSpeed * Time.deltaTime);
            }
            return true;
        }
        else if (InputManager.Instance.GetLookUpKey())
        {
            if(InputManager.Instance.GetLookRightKey())
                //Look top right
            {
                Vector3 newPos = new Vector3(this.deadZonePos.x + this.widthRange / 2 + this.lookHorizontalRange, this.knightPos.position.y + this.lookVerticalRange, this.zAxisPosition);
                transform.position = Vector3.Slerp(transform.position, newPos, this.followSpeed * Time.deltaTime);
            }
            else if(InputManager.Instance.GetLookLeftKey())
                //Look top left
            {
                Vector3 newPos = new Vector3(this.deadZonePos.x + this.widthRange / 2 - this.lookHorizontalRange, this.knightPos.position.y + this.lookVerticalRange, this.zAxisPosition);
                transform.position = Vector3.Slerp(transform.position, newPos, this.followSpeed * Time.deltaTime);
            }
            else
                //Look top only
            {
                Vector3 newPos = new Vector3(this.deadZonePos.x + this.widthRange / 2, this.knightPos.position.y + this.lookVerticalRange, this.zAxisPosition);
                transform.position = Vector3.Slerp(transform.position, newPos, this.followSpeed * Time.deltaTime);
            }
            return true;
        }
        else if (InputManager.Instance.GetLookRightKey())
            //Look right only
        {
            Vector3 newPos = new Vector3(this.deadZonePos.x + this.widthRange / 2 + this.lookHorizontalRange, this.knightPos.position.y + this.heightOffset, this.zAxisPosition);
            transform.position = Vector3.Slerp(transform.position, newPos, this.followSpeed * Time.deltaTime);
            return true;
        }
        else if (InputManager.Instance.GetLookLeftKey())
            //Look left only
        {
            Vector3 newPos = new Vector3(this.deadZonePos.x + this.widthRange / 2 - this.lookHorizontalRange, this.knightPos.position.y + this.heightOffset, this.zAxisPosition);
            transform.position = Vector3.Slerp(transform.position, newPos, this.followSpeed * Time.deltaTime);
            return true;
        }

        return false;
    }

    private bool OutOfDeadZone()
    {
        if (this.knightPos.position.x >= this.rightDeadBorder || this.knightPos.position.x <= this.leftDeadBorder)
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
        Vector3 newPos = new Vector3(knightPos.position.x + widthRange, this.verticalLevel + this.heightOffset, zAxisPosition);
        Vector3 tmpPos = Vector3.Slerp(transform.position, newPos, this.followSpeed * Time.deltaTime);
        transform.position = new Vector3(tmpPos.x, tmpPos.y, this.zAxisPosition);

        //Check if standing
        if (KnightMovement.Instance.Horizontal == 0f && !KnightMovement.Instance.falling)
        {
            this.ResetDeadzone();
        }
    }

    /// <summary>
    /// Other
    /// </summary>
    protected virtual void FindVerticalLevel()
    {
        if (KnightMovement.Instance.IsGround)
        {
            this.verticalLevel = this.knightPos.position.y;
            this.verticalMoveHard = false;
        }
        else if(!this.verticalMoveHard)
        {
            if (this.knightPos.position.y >= this.verticalLevel + this.deadZoneVertical || this.knightPos.position.y <= this.verticalLevel - this.deadZoneVertical)
            {
                this.verticalMoveHard = true;
                //Setup freeze position
                this.relativePos = transform.position - this.knightPos.position;
            }
        }
    }
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
        transform.position = new Vector3(this.knightPos.position.x, this.knightPos.position.y + this.heightOffset, this.zAxisPosition);
    }

    //
    // Cinematic movement
    //
    public IEnumerator FocusToObject(Vector3 position)
    {
        //Set stats
            //Camera
        this.followingKnight = false;
        //Knight
        KnightMovement.Instance.Horizontal = 0f;
        KnightMovement.Instance.RigidBody.velocity = new Vector2(0f, KnightMovement.Instance.RigidBody.velocity.y);
        KnightState.Instance.controlable = false;
            //UI
        this.UIObj.SetActive(false);
            
        //Move to object position
        Vector3 newPos = new Vector3(position.x, position.y, this.zAxisPosition);
        while(Vector3.Distance(transform.position, newPos) > 0.1f)
        {
            transform.position = Vector3.Slerp(transform.position, newPos, this.followSpeed * Time.deltaTime);
            yield return null;
        }
    }
    public IEnumerator FocusToKnight()
    {
        //Wait 1s
        yield return new WaitForSeconds(1f);

        //Fade in screen
        yield return StartCoroutine(this.FadeScreen(1, 1));

        //Reset camera
        this.ResetToKnightPos();

        //Fade out screen
        yield return StartCoroutine(this.FadeScreen(0, 1));

        //Set stats
        this.followingKnight = true;    //Camera
        KnightState.Instance.controlable = true;    //Knight
        this.UIObj.SetActive(true); //UI
    }
    protected IEnumerator FadeScreen(float targetValue, float duration) //Fade screen to target value in a duration
    {
        float startValue = this.fadeScreenCanvas.alpha;
        float time = 0;
        while (time < duration)
        {
            this.fadeScreenCanvas.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        this.fadeScreenCanvas.alpha = targetValue;
    }
}