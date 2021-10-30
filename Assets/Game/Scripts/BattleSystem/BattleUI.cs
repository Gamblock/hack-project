using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    public TextMeshProUGUI nameText;


    public void SetHud(BattleUnit unit)
    {
        nameText.text = unit.unitName;
        hpSlider.maxValue = unit.hp;
        hpSlider.value = unit.hp;
    }

    public void SetHP(int value)
    {
        hpSlider.value = value;
    }
}
