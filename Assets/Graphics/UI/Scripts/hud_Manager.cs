using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HuD_Manager : MonoBehaviour
{
    [SerializeField] private Slider HpBar;
    [SerializeField] private Slider HeatBar;
    [SerializeField] private TextMeshProUGUI HpCounter;
    [SerializeField] private Gradient HeatColor;
    [SerializeField] private Image HeatSliderFill;
    [SerializeField] private TextMeshProUGUI LevelName;

    public void SetMaxHpSliderValue (float MaxHp) 
    {
        HpBar.maxValue = MaxHp;
    }
    public void SetCurrentHpSliderValue (float CurrentHp) 
    {
        HpBar.value = CurrentHp;

        HpCounter.text = ($"{CurrentHp:0.00}/{HpBar.maxValue:0.00}");
    }
    public void SetMaxHeatSliderValue (float MaxHeat)
    {
        HeatBar.maxValue = MaxHeat;
    }    
    public void SetHeatSliderValue (float Heat)
    {
        HeatBar.value = Heat;
    }
    public void SetLevelName (string LevelDisplay)
    {
        LevelName.text = (LevelDisplay);
    }
}
