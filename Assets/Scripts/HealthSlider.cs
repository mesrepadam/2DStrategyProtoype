using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public void SetHealthSlider(int maxHP)
    {
        Slider slider = gameObject.GetComponent<Slider>();
        slider.maxValue = maxHP;
        slider.value = maxHP;
    }


    public void UpdateHealthSlider(int hp)
    {
        Slider slider = gameObject.GetComponent<Slider>();
        slider.value = hp;
    }
}
