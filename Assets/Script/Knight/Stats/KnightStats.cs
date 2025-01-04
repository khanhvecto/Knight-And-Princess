using UnityEngine;

public class KnightStats : MonoBehaviour, HpBarInterface, EnduranceBarInterface
{
    //Design pattern
    private static KnightStats instance;
    public static KnightStats Instance { get => instance; }

    //Endurance
        //Stats
    public float minEndurance = 0f;
    public float maxEndurance = 100f;
    public float endurance = 100f;
        //Chaning speed
    public float enduranceLooseSpeed = 8f;
    public float enduranceRestoreSpeed = 16f;

    [Header("Combat stat")]
    [SerializeField] public float maxHealth = 30f;
    [SerializeField] public float health = 30f;
    [SerializeField] public float damage = 5f;
    [SerializeField] public float cooldown = 1f;
    [SerializeField] public float touchDamage = 3f;

    private void Awake()
    {
        //Design pattern
        if (instance != null) Debug.LogError("Only 1 KnightStat allow to exist!");
        instance = this;
    }

    //
    //Endurance
    //
    public void DecreaseEndurance(float value)
    {
        this.endurance -= value;
        if (this.endurance < this.minEndurance)  //Limit: endurance can not smaller than min value
        {
            this.endurance = this.minEndurance;
        }
    }

    //
    //Health
    //
    public void Heal(float value)
    {
        this.health += value;
        if (this.health > this.maxHealth) this.health = maxHealth;  //Make sure health doesn't exceed max health
    }

    //
    //Slide bar
    //
        //Health
    public float GetHp()
    {
        return this.health;
    }
    public float GetMaxHp()
    {
        return this.maxHealth;
    }
        //Endurance
    public float GetEndurance()
    {
        return this.endurance;
    }
    public float GetMaxEndurance()
    {
        return this.maxEndurance;
    }
}
