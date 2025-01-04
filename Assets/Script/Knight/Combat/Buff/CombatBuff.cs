using System.Collections;
using UnityEngine;

public abstract class CombatBuff : MonoBehaviour
{
    [Header("References")]
        //UI
    [SerializeField] protected BuffHud hudObj;
        //Effect
    [SerializeField] protected GameObject effectObj;
    [SerializeField] protected Animator animator;


    [Header("Stats")]
    [SerializeField] protected int quantity = 0;
        //Effect
    [SerializeField] protected float effectTime;
    [SerializeField] protected string startEffectName = "StartBuff";
    [SerializeField] protected string endEffectName = "EndBuff";

    //State
    protected bool isUsing = false;

    //Buff types
    public enum BuffType
    {
        healthBuff, 
        shieldBuff, 
        damageBuff, 
        invincibleBuff
    }

    protected virtual void Awake()
    {
        this.LoadReferences();
    }
    protected void LoadReferences()
    {
        //Hud obj
        if (this.hudObj == null) Debug.LogError("Can't find hud obj for CombatBuff of " + gameObject.name);
        //Effect object
        this.effectObj = transform.Find("Effect").gameObject;
        if(this.effectObj == null) Debug.LogError("Can't find effect object for Combat Buff of " + gameObject.name);
        //Animator
        this.animator = effectObj.GetComponent<Animator>();
        if (this.animator == null) Debug.LogError("Can't find effect animator for Combat Buff of " + gameObject.name); 
    }

    //
    //Using buff
    //
    public void TryUseBuff()  //Check to use 1 buff in a period of time
    {
        if (this.quantity > 0 && !this.isUsing)
        {
            StartCoroutine(this.UseBuff());
        }
    }
    protected IEnumerator UseBuff()
    {
        this.quantity -= 1;
        this.hudObj.textQuantity.text = this.quantity.ToString();
        this.isUsing = true;

        this.PerformBuff();
        StartCoroutine(this.ShowEffect(this.startEffectName));
        yield return StartCoroutine(this.Buffing());
        this.ResetBuff();
        StartCoroutine(this.ShowEffect(this.endEffectName));

        this.isUsing = false;

        //If there's no buff left, then unshow icon
        if (this.quantity == 0) this.hudObj.gameObject.SetActive(false);
    }

    //
    //Buffs detail
    //
    protected virtual void PerformBuff()
    {
    }
    protected virtual IEnumerator Buffing()
    {

        float startTime = 0f;
        while(startTime < this.effectTime)
        {
            this.hudObj.slider.value = (this.effectTime - startTime) / this.effectTime;
            startTime += Time.deltaTime;
            yield return null;
        }

    }
    protected virtual void ResetBuff()
    {
    }

    //Effect animation
    protected IEnumerator ShowEffect(string name)
    {
        //Set effect object active
        this.effectObj.SetActive(true);

        //Wait to finish effect
        this.animator.Play(name);
        float timer = 0f;
        float waitTime = this.animator.GetCurrentAnimatorClipInfo(0).Length;
        while (timer < waitTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        //Set effect object inactive
        this.effectObj.SetActive(false);
    }

    //
    //Receive buffs
    //
    public void ReceiveBuff(int quantity)
    {
        this.quantity += quantity;
        this.hudObj.textQuantity.text = this.quantity.ToString();
        this.hudObj.gameObject.SetActive(true);
    }
}   
