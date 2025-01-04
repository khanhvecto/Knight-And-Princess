using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffHud : MonoBehaviour
{
    [Header("References")]
    public Slider slider;
    public TextMeshProUGUI textQuantity;

    protected void Awake()
    {
        this.LoadReferences();
    }
    protected void LoadReferences()
    {
        //Slider
        if (this.slider == null) Debug.LogError("Can't find slider for buff hud of " + gameObject.name);
        //Text quantity
        if (this.textQuantity == null) Debug.LogError("Can't find text quantity for buff hud of " + gameObject.name);
    }
}