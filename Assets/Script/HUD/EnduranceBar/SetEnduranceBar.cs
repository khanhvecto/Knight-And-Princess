using UnityEngine;
using UnityEngine.UI;

public class SetEnduranceBar : MonoBehaviour
{
    //References
    [SerializeField] private GameObject objectToShow;
    private EnduranceBarInterface enduranceBarInterface;
    private Slider slider;

    private void Start()
    {
        this.LoadReferences();

        //Init stats
        this.slider.minValue = 0f;
    }
    private void LoadReferences()
    {
        //ObjectToShow
        this.objectToShow = KnightStats.Instance.gameObject;
        if (this.objectToShow == null) Debug.LogError("Can't find ObjectToShow for KnightEndurance");
        //EnduranceBarInterface
        this.enduranceBarInterface = this.objectToShow.GetComponent<EnduranceBarInterface>();
        if (this.enduranceBarInterface == null) Debug.LogError("Can't find EnduranceBarInterface for KnightEndurance");
        //Slider
        this.slider = gameObject.GetComponent<Slider>();
        if (this.slider == null) Debug.LogError("Can't find slider for Knight EnduranceBar");
    }

    void Update()
    {
        this.SetEndurance();
    }
    private void SetEndurance()
    {
        this.slider.maxValue = this.enduranceBarInterface.GetMaxEndurance();
        this.slider.value = this.enduranceBarInterface.GetEndurance();
    }
}
