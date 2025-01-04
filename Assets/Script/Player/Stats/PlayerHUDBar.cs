using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDBar : HUDBar
{
    [Header("References")]
    [SerializeField] protected Slider healthBar;
    [SerializeField] protected Slider enduranceBar;

    protected void Start()
    {
        this.LoadReferences();
        this.InitStats();
    }

    protected void LoadReferences()
    {
        // Health bar
        if (this.healthBar == null)
            Debug.LogError("Can't find health bar for PlayerHUDBar of " + transform.parent.name);
        // Endurance bar
        if (this.enduranceBar == null)
            Debug.LogError("Can't find endurance bar for PlayerHUDBar of " + transform.parent.name);
    }

    protected void InitStats()
    {
        this.healthBar.gameObject.SetActive(true);
    }

    #region Health bar

    public void InitHealthBar(float minValue, float maxValue)
    {
        base.SetBarMinValue(this.healthBar, minValue);
        base.SetBarMaxValue(this.healthBar, maxValue);
    }

    public void SetHealthBarValue(float value)
    {
        base.SetBarValue(this.healthBar, value);
    }

    #endregion
    
    #region Endurance bar

    public void InitEnduranceBar(float minValue, float maxValue)
    {
        base.SetBarMinValue(this.enduranceBar, minValue);
        base.SetBarMaxValue(this.enduranceBar, maxValue);
    }

    public void SetEnduranceBarValue(float value)
    {
        base.SetBarValue(this.enduranceBar, value);
    }

    public void ShowEnduranceBar(bool show)
    {
        this.enduranceBar.gameObject.SetActive(show);
    }

    #endregion
}