using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("--- SINGLETON ---")]
    private static CameraFollow instance;
    public static CameraFollow Instance { get => instance; }

    [Header("--- REFERENCES ---")]
    [SerializeField] protected PlayerStats playerStatsScript;
    [SerializeField] protected Transform playerTransform;
    [SerializeField] protected LayerMask groundLayerMask;

    [Header("--- STATES ---")]
    public bool isFollowingPlayer = true;
    protected bool isFreezingCamera = false;
    [Header("Falling freeze")]
    protected bool isFallingFreeze = false;
    protected bool isMovingToFallingFreezePos = false;
    protected bool isMovedToFallingFreezePos = false;

    [Header("--- STATS ---")]
    protected Vector2 desiredPos;
    protected float zAxisPos = -10f;
    [Header("Camera normal position")]
    public float xAxisOffset;
    public float yAxisOffset;
    [Header("Falling freeze")]
    public float fallOffset;
    public float moveTime;
    [Header("Camera speed")]
    public float smoothSpeed;
    [Header("Player position")]
    public float groundCheckLimit;
    protected float playerVerticalLevel;

    protected void Start()
    {
        this.SetSingleton();
    }

    protected void SetSingleton()
    {
        if(CameraFollow.instance != null)
        {
            Debug.LogError("Only 1 camera follow allowed to exist!");
            Destroy(gameObject);
        }
        instance = this;
    }

    protected void LateUpdate()
    {
        if (this.isFollowingPlayer)
            this.FollowPlayer();
    }

    protected void FollowPlayer()
    {
        this.CheckXAxisOffset();

        if (!this.isFreezingCamera)
            this.FollowPlayerNormal();
        else
            this.FollowPlayerFreezing();
    }

    protected bool IsFreezingCamera()
    {
        // Check if falling from very high place
        if (this.playerVerticalLevel - this.playerTransform.position.y > 0.1)
        {
            RaycastHit2D groundHit = Physics2D.Raycast(this.playerTransform.position, Vector2.down, this.groundCheckLimit, this.groundLayerMask);
            if (groundHit)
            {
                Debug.Log("hit");
                return false;
            }
            else
            {
                Debug.Log("no hit");    
                this.isFallingFreeze = true;
                return true;
            }
        }

        return false;
    }

    protected bool IsUnfreezingCamera()
    {
        // For falling freeze
        if (this.playerStatsScript.isOnGround && this.playerStatsScript.rb2D.velocity.y == 0)
        {
            this.isFallingFreeze = false;
            return true;
        }
        
        return false;
    }

    #region Normal follow
    protected void FollowPlayerNormal()
    {

        if (IsFreezingCamera())
        {
            this.isFreezingCamera = true;
            return;
        }

        this.FindPlayerVerticalLevel();

        this.desiredPos = new Vector2(this.playerTransform.position.x + this.xAxisOffset, this.playerVerticalLevel + this.yAxisOffset);
        var lerpPos = Vector2.Lerp(transform.position, desiredPos, this.smoothSpeed);
        var smoothedPos = new Vector3(lerpPos.x, lerpPos.y, this.zAxisPos);
        transform.transform.position = smoothedPos;
    }

    protected void FindPlayerVerticalLevel()
    {
        if(this.playerStatsScript.isOnGround && this.playerStatsScript.rb2D.velocity.y == 0)
            this.playerVerticalLevel = this.playerTransform.position.y;
    }

    #endregion

    #region Freezing follow
    protected void FollowPlayerFreezing()
    {
        if (this.IsUnfreezingCamera())
            this.ExitFreezeState();
        
        if(this.isFallingFreeze)
        {
            if(!this.isMovingToFallingFreezePos && !this.isMovedToFallingFreezePos)
                this.StartCoroutine(this.MoveToFallingFreezePos());
            if(this.isMovedToFallingFreezePos)
            {
                var desiredPos = this.desiredPos = new Vector2(this.playerTransform.position.x + this.xAxisOffset, this.playerTransform.position.y + this.fallOffset);
                var lerpPos = Vector2.Lerp(transform.position, desiredPos, this.smoothSpeed);
                transform.position = new Vector3(lerpPos.x, this.playerTransform.position.y + this.fallOffset, this.zAxisPos);
            }
        }
    }

    protected IEnumerator MoveToFallingFreezePos()
    {
        this.isMovingToFallingFreezePos = true;

        var startPointVertical = this.playerVerticalLevel + this.yAxisOffset;
        float timePassed = 0f;
        while(true)
        {
            timePassed += Time.deltaTime;

            var endPointVertical = this.playerTransform.position.y + this.fallOffset;
            var desiredPosVertical = startPointVertical - (startPointVertical - endPointVertical) / this.moveTime * timePassed;
            this.desiredPos = new Vector2(this.playerTransform.position.x + this.xAxisOffset, desiredPosVertical);
            var lerpPos = Vector2.Lerp(transform.position, desiredPos, this.smoothSpeed);

            transform.position = new Vector3(lerpPos.x, desiredPosVertical, zAxisPos);

            // If clome to desired pos vertical, exit Moving
            if (timePassed >= this.moveTime)
            {
                this.isMovingToFallingFreezePos = false;
                this.isMovedToFallingFreezePos = true;
                yield break;
            }

            if(this.IsUnfreezingCamera())
            {
                this.ExitFreezeState();
                yield break;
            }

            yield return null;
        }
    }

    protected void ExitFreezeState()
    {
        this.isFreezingCamera = false;
        this.ResetFreezeParameters();
        return;
    }

    protected void ResetFreezeParameters()
    {
        // Falling freeze
        this.isMovedToFallingFreezePos = false;
        this.isMovingToFallingFreezePos = false;
    }

    #endregion

    protected void CheckXAxisOffset()
    {
        if ((this.playerStatsScript.isFacingRight && this.xAxisOffset < 0)
            || (!this.playerStatsScript.isFacingRight && this.xAxisOffset > 0))
        {
            this.xAxisOffset = -this.xAxisOffset;
        }
    }
}
