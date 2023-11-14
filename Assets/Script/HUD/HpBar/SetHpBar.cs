using UnityEngine;
using UnityEngine.UI;

public class SetHpBar : MonoBehaviour
{
    [SerializeField] private GameObject objectToShowHp;
    private HpBarInterface hpBarInterface;
    private Slider slider;

    private void Start()
    {
        this.LoadReferences();
        
        //Init stats
        slider.minValue = 0f;
    }
    private void LoadReferences()   //Load and check if any reference null
    {
        //ObjectToShowHp
        if (this.objectToShowHp == null) Debug.LogError("Can't find ObjectToShowHp for SetHpBar");
        //hpBarInterface
        this.hpBarInterface = this.objectToShowHp.GetComponent<HpBarInterface>();
        if (this.hpBarInterface == null) Debug.LogError("Can't find hpBarInterFace for SetHpBar");
        //Slider
        slider = gameObject.gameObject.GetComponent<Slider>();
        if (slider == null) Debug.LogError("Can't find slider for SetHpBar");
    }

    void FixedUpdate()
    {
        UpdateHp();
    }

    private void UpdateHp() //Update Hp every frame
    {
        this.slider.maxValue = this.hpBarInterface.GetMaxHp();
        this.slider.value = this.hpBarInterface.GetHp();
    }
}
