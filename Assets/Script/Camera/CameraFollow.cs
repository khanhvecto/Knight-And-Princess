using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("--- SINGLETON ---")]
    private static CameraFollow instance;
    public static CameraFollow Instance { get => instance; }

    [Header("--- REFERENCES ---")]
    [SerializeField] protected PlayerStats playerStatsScript;
    [SerializeField] protected PlayerMovement playerMovementScript;
    [SerializeField] protected Transform playerTransform;
    [SerializeField] protected LayerMask groundLayerMask;
    [SerializeField] protected GameObject playerHUDObj;
    [SerializeField] protected CanvasGroup fadeScreenCanvas;

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
    public float yAxisOffsetDefault;
    protected float yAxisOffset;
    [Header("Falling freeze")]
    public float fallOffset;
    public float moveTime;
    [Header("Camera speed")]
    public float smoothSpeed;
    [Header("Player position")]
    public float groundCheckLimit;
    protected float playerVerticalLevel;
    [Header("Look further")]
    public float yAxisOffsetLookUp;
    public float yAxisOffsetLookDown;
    [Header("Focus to object")]
    public float focusSpeed;

    protected void Start()
    {
        this.SetSingleton();
        this.InitStats();
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

    protected void InitStats()
    {
        this.yAxisOffset = this.yAxisOffsetDefault;
        this.transform.position = this.playerTransform.position;
        this.playerVerticalLevel = this.playerTransform.position.y;
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
        if (this.playerVerticalLevel - this.playerTransform.position.y > 2)
        {
            RaycastHit2D groundHit = Physics2D.Raycast(this.playerTransform.position, Vector2.down, this.groundCheckLimit, this.groundLayerMask);
            if (groundHit)
            {
                return false;
            }
            else
            {
                this.isFallingFreeze = true;
                return true;
            }
        }

        return false;
    }

    protected bool IsUnfreezingCamera()
    {
        // For falling freeze
        if (this.playerStatsScript.isOnGround)
        {
            this.isFallingFreeze = false;
            return true;
        }
        
        return false;
    }

    #region Normal follow
    protected void FollowPlayerNormal()
    {
        this.FindPlayerVerticalLevel();

        if (IsFreezingCamera())
        {
            this.isFreezingCamera = true;
            return;
        }

        this.desiredPos = new Vector2(this.playerTransform.position.x + this.xAxisOffset, this.playerVerticalLevel + this.yAxisOffset);
        var lerpPos = Vector2.Lerp(transform.position, desiredPos, this.smoothSpeed);
        var smoothedPos = new Vector3(lerpPos.x, lerpPos.y, this.zAxisPos);
        transform.transform.position = smoothedPos;
    }

    protected void FindPlayerVerticalLevel()
    {
        if(this.playerStatsScript.isOnGround)
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

        var startPointVertical = transform.position.y;
        float timePassed = 0f;
        while(true)
        {
            timePassed += Time.deltaTime;

            var endPointVertical = this.playerTransform.position.y + this.fallOffset;
            var desiredPosVertical = startPointVertical - (startPointVertical - endPointVertical) / this.moveTime * timePassed;
            this.desiredPos = new Vector2(this.playerTransform.position.x + this.xAxisOffset, desiredPosVertical);
            var lerpPos = Vector2.Lerp(transform.position, desiredPos, this.smoothSpeed);

            transform.position = new Vector3(lerpPos.x, desiredPosVertical, zAxisPos);

            // If come to desired pos vertical, exit Moving
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

    public void ExitFreezeState()
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

    #region Set look further

    public void SetLookUp(bool set)
    {
        if (set)
            this.yAxisOffset = this.yAxisOffsetLookUp;
        else
            this.yAxisOffset = this.yAxisOffsetDefault;
    }

    public void SetLookDown(bool set)
    {
        if (set)
            this.yAxisOffset = this.yAxisOffsetLookDown;
        else
            this.yAxisOffset = this.yAxisOffsetDefault;
    }

    #endregion

    #region Cut scene movement
    public IEnumerator SetMoveToPos(Vector3 position)
        // Focus to a specific position like a cutscene
        // Need to focus back to player by below function
    {
        //Set knight stays steady
        this.playerMovementScript.StopMoving();
        this.playerStatsScript.controlAbility = false;
        //UI
        this.playerHUDObj.SetActive(false);

        //Move to object position
        yield return StartCoroutine(this.MoveToPos(position));
    }

    public IEnumerator MoveToPos(Vector3 pos)
    {
        //Camera state
        this.isFollowingPlayer = false;

        //Moving
        Vector3 newPos = new Vector3(pos.x, pos.y, this.zAxisPos);
        while (Vector3.Distance(transform.position, newPos) > 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, this.focusSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator FocusToKnight()
    {
        var knightPos = new Vector3(this.playerTransform.position.x, this.playerTransform.position.y + this.yAxisOffset, this.zAxisPos);
        yield return StartCoroutine(this.FadeToPos(knightPos));

        //Set stats
        this.playerStatsScript.controlAbility = true;
        this.playerHUDObj.SetActive(true); //UI
        this.isFollowingPlayer = true;
    }

    public IEnumerator FadeToPos(Vector3 newPos)
    {
        this.isFollowingPlayer = false;

        //Fade in screen
        yield return StartCoroutine(this.FadeScreen(1, 1));

        //Reset camera
        transform.position = newPos;

        //Fade out screen
        yield return StartCoroutine(this.FadeScreen(0, 1));
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

    #endregion

    #region Shake screen

    public void ShakeCamera(float duration, float amplitude)
    {
        StartCoroutine(this.Shaking(duration, amplitude));
    }

    protected IEnumerator Shaking(float duration, float amplitude)
    {
        float timer = 0f;
        Vector3 oldOffset = Vector3.zero;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            float randX = Random.Range(-1, 1) * amplitude;
            float randY = Random.Range(-1, 1) * amplitude;
            Vector3 shakeOffset = new Vector3(randX, randY, 0f);
            transform.position =  transform.position - oldOffset + shakeOffset;
            oldOffset = shakeOffset;

            yield return null;
        }
    }

    #endregion
}
