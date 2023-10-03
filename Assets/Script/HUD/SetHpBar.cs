using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHpBar : MonoBehaviour
{
    [SerializeField] private GameObject objectToShowHp;
    private Slider slider;

    private void Awake()
    {
        slider = gameObject.gameObject.GetComponent<Slider>();
        if(slider != null )
        {
            slider.minValue = 0;
        }
        else
        {
            Debug.LogWarning("There's no slider for hp bar");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateHp();
    }

    private void UpdateHp()
    {
        if(slider == null) { return; }

        HpBarInterface hpBarInterface = objectToShowHp.GetComponent<HpBarInterface>();
        if(hpBarInterface == null ) { return; }

        slider.maxValue = hpBarInterface.GetMaxHp();
        slider.value = hpBarInterface.GetHp();
    }
}
