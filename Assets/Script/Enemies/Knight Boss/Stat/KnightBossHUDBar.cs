using UnityEngine;
using UnityEngine.UI;

public class KnightBossHUDBar : HUDBar
{
    [Header("References")]
    [SerializeField] protected Slider healthBar;
    [SerializeField] protected KnightBossStats statsScript;

    protected void Update()
    {
        this.CheckDisplayHealthBar();
    }

    protected void CheckDisplayHealthBar()
    {
        if (this.statsScript.isCombating && !this.healthBar.gameObject.activeSelf)
            this.healthBar.gameObject.SetActive(true);
        else if (!this.statsScript.isCombating && this.healthBar.gameObject.activeSelf)
            this.healthBar.gameObject.SetActive(false);
    }

    public void SetHealthBarValue(float value)
    {
        base.SetBarValue(this.healthBar, value);
    }
    
    public void SetHealthBarMaxValue(float value)
    {
        base.SetBarMaxValue(this.healthBar, value);
    }
    
    public void SetHealthBarMinValue(float value)
    {
        base.SetBarMinValue(this.healthBar, value);
    }
}
