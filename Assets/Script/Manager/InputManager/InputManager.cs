using UnityEngine;

public class InputManager : MonoBehaviour
{
    // singleton
    private static InputManager instance;
    public static InputManager Instance { get => instance; }

    private void Start()
    {
        // singleton
        if (instance != null) Debug.LogError("Only 1 InputManager allows to exist!");
        instance = this;
    }

    #region Camera
    private KeyCode lookDownKey = KeyCode.S;
    private KeyCode lookUpKey = KeyCode.W;
    private KeyCode lookRightKey = KeyCode.RightArrow;
    private KeyCode lookLeftKey = KeyCode.LeftArrow;
    public bool GetLookDownKey()
    {
        if (Input.GetKey(this.lookDownKey)) return true;
        return false;
    }
    public bool GetLookUpKey()
    {
        if (Input.GetKey(this.lookUpKey)) return true;
        return false;
    }
    public bool GetLookRightKey()
    {
        if (Input.GetKey(this.lookRightKey)) return true;
        return false;
    }
    public bool GetLookLeftKey()
    {
        if (Input.GetKey(this.lookLeftKey)) return true;
        return false;
    }
    #endregion

    #region Player movement

    // Jump
    public KeyCode jumpKey = KeyCode.Space;
    public bool GetJumpKeyDown()
    {
        if (Input.GetKeyDown(this.jumpKey)) return true;
        return false;
    }
    public bool GetJumpKey()
    {
        if (Input.GetKey(this.jumpKey)) return true;
        return false;
    }
    public bool GetJumpKeyUp()
    {
        if (Input.GetKeyUp(this.jumpKey)) return true;
        return false;
    }

    // roll
    private KeyCode rollKey = KeyCode.LeftShift;
    public bool GetRollKeyDown()
    {
        if (Input.GetKeyDown(this.rollKey)) return true;
        return false;
    }

    // Sprint
    public KeyCode sprintKey = KeyCode.LeftControl;
    public bool GetSprintKeyDown()
    {
        if(Input.GetKeyDown(this.sprintKey)) return true;
        return false;
    }
    public bool GetSprintKey()
    {
        if(Input.GetKey(this.sprintKey)) return true;
        return false;
    }
    public bool GetSprintKeyUp()
    {
        if (Input.GetKeyUp(this.sprintKey)) return true;
        return false;
    }

    #endregion

    #region Player combat

    //Attack
    private KeyCode attackKey = KeyCode.Mouse0;
    public bool GetAttackKeyDown()
    {
        if (Input.GetKeyDown(this.attackKey)) return true;
        return false;
    }

    //Block
    private KeyCode blockKey = KeyCode.Mouse1;
    public bool GetBlockKeyDown()
    {
        if (Input.GetKeyDown(this.blockKey)) return true;
        return false;
    }
    public bool GetBlockKey()
    {
        if (Input.GetKey(this.blockKey)) return true;
        return false;
    }
    public bool GetBlockKeyUp()
    {
        if (Input.GetKeyUp(this.blockKey)) return true;
        return false;
    }

    //Buffs
    private KeyCode healthBuffKey = KeyCode.Alpha1;
    private KeyCode shieldBuffKey = KeyCode.Alpha2;
    private KeyCode damageBuffKey = KeyCode.Alpha3;
    private KeyCode invincibleBuffKey = KeyCode.Alpha4;
    public bool GetHealthBuffKeyDown()
    {
        if (Input.GetKeyDown(this.healthBuffKey)) return true;
        return false;
    }
    public bool GetShieldBuffKeyDown()
    {
        if (Input.GetKeyDown(this.shieldBuffKey)) return true;
        return false;
    }
    public bool GetDamageBuffKeyDown()
    {
        if (Input.GetKeyDown(this.damageBuffKey)) return true;
        return false;
    }
    public bool GetInvincibleBuffKeyDown()
    {
        if (Input.GetKeyDown(this.invincibleBuffKey)) return true;
        return false;
    }
    #endregion

    #region Interactable Object
    private KeyCode interactKey = KeyCode.F;
    public bool GetInteractKeyDown()
    {
        if(Input.GetKeyDown(this.interactKey)) return true;
        return false;
    }
    #endregion

    #region Pause game

    [SerializeField] protected KeyCode pauseGameKey = KeyCode.Escape;

    public bool GetPauseGameKeyDown()
    {
        if(Input.GetKeyDown(this.pauseGameKey)) return true;
        return false;
    }

    #endregion
}
