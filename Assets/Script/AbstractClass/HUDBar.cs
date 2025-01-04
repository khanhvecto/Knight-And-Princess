using UnityEngine;
using UnityEngine.UI;

public abstract class HUDBar : MonoBehaviour
{
    protected void SetBarValue(Slider slider, float value)
    {
        slider.value = value;
    }

    protected void SetBarMaxValue(Slider slider, float maxValue)
    {
        slider.maxValue = maxValue;
    }

    protected void SetBarMinValue(Slider slider, float minValue)
    {
        slider.minValue = minValue;
    }
}