using UnityEngine;

public class InputManager : MonoBehaviour
{
    //Design pattern
    private static InputManager instance;
    public static InputManager Instance { get => instance; }

    //Camera
    private KeyCode lookDownKey = KeyCode.DownArrow;
    private KeyCode lookUpKey = KeyCode.UpArrow;
    private KeyCode lookRightKey = KeyCode.RightArrow;
    private KeyCode lookLeftKey = KeyCode.LeftArrow;

    //Attack
    private KeyCode attackKey = KeyCode.Mouse0;

    //Block
    private KeyCode blockKey = KeyCode.Mouse1;

    //Roll
    private KeyCode rollKey = KeyCode.LeftShift;

    //Buffs
    private KeyCode healthBuffKey = KeyCode.Alpha1;
    private KeyCode shieldBuffKey = KeyCode.Alpha2;
    private KeyCode damageBuffKey = KeyCode.Alpha3;
    private KeyCode invincibleBuffKey = KeyCode.Alpha4;

    //Interactable Object
    private KeyCode interactKey = KeyCode.F;

    private void Start()
    {
        //Design pattern
        if (instance != null) Debug.LogError("Only 1 InputManager allows to exist!");
        instance = this;
    }

    //Camera
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

    //Knight attack
    public bool GetAttackKeyDown()
    {
        if (Input.GetKeyDown(this.attackKey)) return true;
        return false;
    }

    //Knight block
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

    //Knight roll
    public bool GetRollKeyDown()
    {
        if(Input.GetKeyDown(this.rollKey)) return true;
        return false;
    }

    //Buffs
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


    //Interactable Object
    public bool GetInteractKeyDown()
    {
        if(Input.GetKeyDown(this.interactKey)) return true;
        return false;
    }
}
